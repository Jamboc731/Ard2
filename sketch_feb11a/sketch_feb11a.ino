const int playerOne = A0;
const int playerTwo = A1;

const int p1Red = 3;
const int p1Gren = 5;
const int p2Gren = 6;
const int p2Red = 9;


int oneVal;
int twoVal;
int p1Mil;
int p2Mil;
int curMil = 0;
int incomingByte;

bool p1Flash;
bool p2Flash;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(p1Red, OUTPUT);
  pinMode(p1Gren, OUTPUT);
  pinMode(p2Red, OUTPUT);
  pinMode(p2Gren, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  curMil = millis();
  
  
  if(Serial.available() > 0){
    incomingByte = Serial.read();
    if(incomingByte == 'p'){
      SendPositions();
    }
    if(incomingByte == 'a'){
      digitalWrite(p1Red, HIGH);
    }
    if(incomingByte == 's'){
      digitalWrite(p1Gren, HIGH);
    }
    if(incomingByte == 'd'){
      digitalWrite(p2Gren, HIGH);
    }
    if(incomingByte == 'f'){
      digitalWrite(p2Red, HIGH);
    }
    if(incomingByte == 'z'){
      digitalWrite(p1Red, LOW);
    }
    if(incomingByte == 'x'){
      digitalWrite(p1Gren, LOW);
    }
    if(incomingByte == 'c'){
      digitalWrite(p2Red, LOW);
    }
    if(incomingByte == 'v'){
      digitalWrite(p2Gren, LOW);
    }
    if(incomingByte == 'r'){
      Reset();
    }
  }
  
}

void Reset()
{
  digitalWrite(p1Red, LOW); 
  digitalWrite(p1Gren, LOW); 
  digitalWrite(p2Red, LOW); 
  digitalWrite(p2Gren, LOW);
}

void SendPositions(){
  oneVal = analogRead(playerOne);
  twoVal = analogRead(playerTwo);

  Serial.print(oneVal);
  Serial.print("-");
  Serial.println(twoVal);
}


