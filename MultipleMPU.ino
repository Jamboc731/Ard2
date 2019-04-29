#include "I2Cdev.h"
#include "MPU6050_6Axis_MotionApps20.h"
#include "Wire.h"

MPU6050 mpu2(0x68);
MPU6050 mpu1(0x69);

bool switchSensor = false;
bool dmpReady = false;

volatile bool interrupt = false;
float euler1[3];
float euler2[3];

float eulerTot[6];

uint16_t fifocount1;
uint16_t packetSize1;
uint8_t fifoBuffer1[64];
uint16_t fifocount2;
uint16_t packetSize2;
uint8_t fifoBuffer2[64];
uint8_t devStatus; 

Quaternion q1;
Quaternion q2;



void dmpDataReady() {
    interrupt = true;
}

void setup() {
    // join I2C bus (I2Cdev library doesn't do this automatically)
    #if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
        Wire.begin();
        TWBR = 24; // 400kHz I2C clock (200kHz if CPU is 8MHz)
    #elif I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE
        Fastwire::setup(400, true);
    #endif

    // initialize serial communication
    // (115200 chosen because it is required for Teapot Demo output, but it's
    // really up to you depending on your project)
    Serial.begin(9600);
    while (!Serial); // wait for Leonardo enumeration, others continue immediately

    Serial.println(F("Initializing I2C device 1"));
    mpu1.initialize();

    // verify connection
    Serial.println(F("Testing device connections..."));
    Serial.println(mpu1.testConnection() ? F("MPU6050 1 connection successful") : F("MPU6050 1 connection failed"));

    // load and configure the DMP
    Serial.println(F("Initializing DMP..."));
    devStatus = mpu1.dmpInitialize();

    // supply your own gyro offsets here, scaled for min sensitivity
    mpu1.setXGyroOffset(220);
    mpu1.setYGyroOffset(76);
    mpu1.setZGyroOffset(-85);
    mpu1.setZAccelOffset(1788); // 1688 factory default for my test chip

    // make sure it worked (returns 0 if so)
    if (devStatus == 0) {
        // turn on the DMP, now that it's ready
        Serial.println(F("Enabling DMP..."));
        mpu1.setDMPEnabled(true);

        // enable Arduino interrupt detection
        Serial.println(F("Enabling interrupt detection (Arduino external interrupt 0)..."));
        attachInterrupt(0, dmpDataReady, RISING);
        interrupt = mpu1.getIntStatus();

        // set our DMP Ready flag so the main loop() function knows it's okay to use it
        Serial.println(F("DMP ready! Waiting for first interrupt..."));
        dmpReady = true;

        // get expected DMP packet size for later comparison
        packetSize1 = mpu1.dmpGetFIFOPacketSize();
    } else {
        // ERROR!
        // 1 = initial memory load failed
        // 2 = DMP configuration updates failed
        // (if it's going to break, usually the code will be 1)
        Serial.print(F("DMP Initialization failed (code "));
        Serial.print(devStatus);
        Serial.println(F(")"));
    }

    
    // initialize device
    Serial.println(F("Initializing I2C device 2"));
    mpu2.initialize();

    // verify connection
    Serial.println(F("Testing device connections..."));
    Serial.println(mpu2.testConnection() ? F("MPU6050 2 connection successful") : F("MPU6050 2 connection failed"));

    // wait for ready
    Serial.println(F("\nSend any character to begin DMP programming and demo: "));
    while (Serial.available() && Serial.read()); // empty buffer
    while (!Serial.available());                 // wait for data
    while (Serial.available() && Serial.read()); // empty buffer again

    // load and configure the DMP
    Serial.println(F("Initializing DMP..."));
    devStatus = mpu2.dmpInitialize();

    // supply your own gyro offsets here, scaled for min sensitivity
    mpu2.setXGyroOffset(220);
    mpu2.setYGyroOffset(76);
    mpu2.setZGyroOffset(-85);
    mpu2.setZAccelOffset(1788); // 1688 factory default for my test chip

    // make sure it worked (returns 0 if so)
    if (devStatus == 0) {
        // turn on the DMP, now that it's ready
        Serial.println(F("Enabling DMP..."));
        mpu2.setDMPEnabled(true);

        // get expected DMP packet size for later comparison
        packetSize2 = mpu2.dmpGetFIFOPacketSize();
    } else {
        // ERROR!
        // 1 = initial memory load failed
        // 2 = DMP configuration updates failed
        // (if it's going to break, usually the code will be 1)
        Serial.print(F("DMP Initialization failed (code "));
        Serial.print(devStatus);
        Serial.println(F(")"));
    }
}

void loop() {
  
  interrupt = mpu1.getIntStatus();
  
  fifocount1 = mpu1.getFIFOCount();
  fifocount2 = mpu2.getFIFOCount();

  mpu1.getFIFOBytes(fifoBuffer1, packetSize1);
  mpu2.getFIFOBytes(fifoBuffer2, packetSize2);

  if(fifocount1 == 1024){
    mpu1.resetFIFO();
  }
  if(fifocount2 == 1024){
    mpu2.resetFIFO();
  }
  
  if(interrupt){
    mpu1.getFIFOBytes(fifoBuffer1, packetSize1);
    mpu1.dmpGetQuaternion(&q1, fifoBuffer1);
    fifocount1 -= packetSize1;
    mpu1.dmpGetEuler(euler1, &q1);
    
    mpu2.getFIFOBytes(fifoBuffer2, packetSize2);
    mpu2.dmpGetQuaternion(&q2, fifoBuffer2);
    fifocount2 -= packetSize2;
    mpu2.dmpGetEuler(euler2, &q2);
 
    interrupt = false;
    }  
  else{
    Serial.println("noInterrupt");
  }
  if(Serial.available()){
    if(Serial.read() == 'p'){
      SendPositions();
    }
  }
  

  //Serial.println("Working");
}

void SendPositions(){

  Serial.print(euler1[0] * 180/M_PI);
  Serial.print(",");
  Serial.print(euler1[1] * 180/M_PI);
  Serial.print(",");
  Serial.print(euler1[2] * 180/M_PI);
  Serial.print("-");
  Serial.print(euler2[0] * 180/M_PI);
  Serial.print(",");
  Serial.print(euler2[1] * 180/M_PI);
  Serial.print(",");
  Serial.println(euler1[2] * 180/M_PI);
  
}
