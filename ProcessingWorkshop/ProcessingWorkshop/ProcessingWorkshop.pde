
import codeanticode.syphon.*;

PGraphics canvas;
SyphonServer server;

int x = 50;
float boxSize = 0;

void settings(){
  size(512,512, P3D);
  PJOGL.profile = 1;
  
  fill(255,0,0);
  background(0);
  rect(40,40,20,20);
  server = new SyphonServer(this, "ProcessingSyphon");
}

void draw(){
 
 
 
 server.sendScreen();
}