int WIDTH = 77;
int HEIGHT = 13;
int HEART_SIZE = 4;
int HEART_MARGIN = 1;

int NUM_HEARTS_X = WIDTH / (HEART_SIZE + HEART_MARGIN);
int NUM_HEARTS_Y = HEIGHT / (HEART_SIZE + HEART_MARGIN);

int[] beats = new int[NUM_HEARTS_X * NUM_HEARTS_Y];

void setup(){
    size(WIDTH,HEIGHT);
}

void draw(){
  background(0);
  
  fill(255,0,0);
  int index =0;
  for (int x=0; x < NUM_HEARTS_X; x++){
    for (int y=0; y < NUM_HEARTS_Y; y++){
      if (
      index++;
    }
  }
}