import processing.serial.*;

Serial serialPort;     
PImage topTexture;
PImage bottomTexture;
PFont font;

float alpha = 0.2;
float prevX, prevY, prevZ;
float pitch, roll;

boolean filter = true;

void setup()
{
  size(1024, 768, P3D);
  
  serialPort = new Serial(this, "COM17", 9600);
  serialPort.clear();
  
  topTexture = loadImage("topTexture.jpg");
  bottomTexture = loadImage("bottomTexture.jpg");
  
  font = createFont("Arial", 16, true);
}

void keyPressed() 
{
  if (key == 'F' || key == 'f')
    filter = !filter;
}

void draw()
{
  GetSample();
  background(255);

  lights();
  directionalLight(126, 126, 126, 0, 0, -1);
  ambientLight(102, 102, 102);
  
  //fill(255, 255, 255);
  fill(0, 0, 0);
  textFont(font);
  text("Pitch: " + nfs(degrees(pitch), 0, 2) + "°" +
       " Roll: " + nfs(degrees(roll), 0, 2) + "°" +
       " [F]ilter: " + (filter ? "on" : "off")
       , 10, 30); 

  if(mousePressed) 
  {
    ortho(0, width, 0, height); 
  } 
  else 
  {
    float fov = PI/3.0; 
    float cameraZ = (height/2.0) / tan(fov/2.0); 
    perspective(fov, float(width)/float(height), cameraZ/2.0, cameraZ*2.0);
  }
    
  pushMatrix(); 

  translate(width/2, height/2, -30); 

  rotateX(pitch); 
  rotateZ(roll); 

  scale(50);
  
  noStroke();
  fill(50, 50, 50);
  
  beginShape();
  texture(topTexture);
  vertex(-3.25, -0.1,  4.75,   0, 265);
  vertex( 3.25, -0.1,  4.75, 179, 265);
  vertex( 3.25, -0.1, -4.75, 179,   0); 
  vertex(-3.25, -0.1, -4.75,   0,   0);
  endShape();

  beginShape();
  vertex(-3.25,  0.1,  4.75);
  vertex( 3.25,  0.1,  4.75);
  vertex( 3.25, -0.1,  4.75);
  vertex(-3.25, -0.1,  4.75);
  endShape();
  
  beginShape();
  vertex(3.25,  0.1,  4.75);
  vertex(3.25,  0.1, -4.75);
  vertex(3.25, -0.1, -4.75);
  vertex(3.25, -0.1,  4.75);
  endShape();

  beginShape();
  vertex( 3.25,  0.1, -4.75);
  vertex(-3.25,  0.1, -4.75);
  vertex(-3.25, -0.1, -4.75);
  vertex( 3.25, -0.1, -4.75);
  endShape();

  beginShape();
  vertex(-3.25,  0.1, -4.75);
  vertex(-3.25,  0.1,  4.75);
  vertex(-3.25, -0.1,  4.75);
  vertex(-3.25, -0.1, -4.75);
  endShape();

  beginShape();
  texture(bottomTexture);
  vertex(-3.25, 0.1,  4.75,   0, 267);
  vertex( 3.25, 0.1,  4.75, 179, 267);
  vertex( 3.25, 0.1, -4.75, 179,   0);
  vertex(-3.25, 0.1, -4.75,   0,   0);
  endShape();
  
  popMatrix(); 
}

void GetSample()
{
  if(serialPort.available() <= 0)
    return;
    
  String msg = serialPort.readStringUntil('\n');
  if (msg == null)
   return; 
   
  msg = trim(msg);
  
  float[] values = float(split(msg, ';'));
  if(values.length < 3)
    return;
  
  float x = values[0]; 
  float y = values[1];
  float z = values[2];

  //low pass filter
  if(filter)
  {
    x = prevX + alpha * (x - prevX);
    y = prevY + alpha * (y - prevY);
    z = prevZ + alpha * (z - prevZ);
  }
  
  roll  = atan2(y, z);
  pitch = -atan2(x, sqrt(sq(y) + sq(z)));
  
  prevX = x;
  prevY = y;
  prevZ = z;
}

