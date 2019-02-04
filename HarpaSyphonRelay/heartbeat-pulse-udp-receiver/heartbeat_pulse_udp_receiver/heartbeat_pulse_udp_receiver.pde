import hypermedia.net.*;

int PORT_RX=5050;
String HOST_IP = "127.0.0.1";
UDP udp;

Cell[][] grid; 

int cols = 77; 
int rows = 13; 
Cell currentCell; 

int lastPulse = 0;

void setup() { 
  size(770,130); 
  grid = new Cell[cols][rows]; 
  for (int i = 0; i < cols; i++) { 
    for (int j = 0; j < rows; j++) { 
      grid[i][j] = new Cell(i*10,j*10,10,10); 
    } 
  }
  udp= new UDP(this, PORT_RX, HOST_IP);
  udp.log(false);
  udp.listen(true);

} 

void draw() { 
  background(0); 
  for (int i = 0; i < cols; i++) { 
    for (int j = 0; j < rows; j++) { 
      grid[i][j].display(); 
    } 
  } 
} 

class Cell { 
  float x,y; // x,y location 
  float w,h; // width and height 
  int bpm = 0; 
  boolean searching; 
  
  
  Cell(float tempX, float tempY, float tempW, float tempH) { 
    x = tempX; 
    y = tempY; 
    w = tempW; 
    h = tempH; 
  } 
  
  
  void display() { 
    noStroke(); 
    
    
    if(searching){ 
      float period = (60.0 / 120) * 1000; 
      float angle = map(millis() % period, 0, period, 0, TWO_PI); 
      fill(122+122*sin(angle),0,0);
      
    } 
    else{ 
      float period = (60.0 / bpm) * 1000; 
      float angle = map(millis() % period, 0, period, 0, TWO_PI); 
      if (bpm == 0){
        fill(0);
      } else {
        fill(127+127*sin(angle));  
      }
       
      
    
    } 
  
  
    rect(x,y,w,h); 
  } 
} 

void keyPressed(){ 
  
  if (key == '1'){ 
  currentCell = FindEmptyCell(); 
  println("Found empty cell");
  } 
  
  if (key == '2'){ 
  currentCell.searching = true;
  println("set current cell to searching");
  } 
  
  if (key == '3'){ 
  currentCell.bpm = lastPulse;
  println("Setting current cell bpm to " + lastPulse);
  } 
  
  if (key == '4'){ 
  currentCell.searching = false; 
  println("set current cell to not searching");
  } 


} 

Cell FindEmptyCell(){ 

boolean hasFoundCell = false; 
int randomCol = 0; 
int randomRow = 0; 
int numTries = 0; 
while(hasFoundCell == false && numTries < 8){ 
randomCol = floor(random(cols)); 
randomRow = floor(random(rows)); 
hasFoundCell = grid[randomCol][randomRow].bpm == 0; 
if (randomCol < 38){ 
int maxRow = floor(map(randomCol, 0, 38, 4, 9)); 
hasFoundCell = randomRow < maxRow; 
} 
numTries = numTries + 1; 
} 

return grid[randomCol][randomRow]; 
} 

void receive(byte[] data, String HOST_IP, int PORT_RX){
  
  // Here we receive the data from the webcam application 
  String raw = new String(data);
  float pulseRate = parseFloat(raw);
  lastPulse = floor(pulseRate);
  println(pulseRate);
}
