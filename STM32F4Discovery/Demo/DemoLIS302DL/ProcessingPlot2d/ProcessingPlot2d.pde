import processing.serial.*;

Serial serialPort;
int lf = 10;
int maxSamples = 500;
int sampleIndex = 0;
int margin = 10;

float[][] samples = new float[3][maxSamples];
int[] colors = new int[]{0xFFFF0000, 0xFF00FF00, 0xFF0000FF};


void setup ()
{
  size(800, 600, P2D);
  background(0);
  
  serialPort = new Serial(this, "COM17", 9600);
  serialPort.clear();
}

void draw() 
{
  background(0);
  
  DrawAxis();
  DrawSamples();
}

void DrawAxis()
{
  fill(colors[0]);
  rect(20, 20, 20, 10);
  fill(255);
  text(" - X", 43, 30);
  
  fill(colors[1]);
  rect(20, 40, 20, 10);
  fill(255);
  text(" - Y", 43, 50);
  
  fill(colors[2]);
  rect(20, 60, 20, 10);
  fill(255);
  text(" - Z", 43, 70);
  
  noFill();
  stroke(0x66);
  rect(margin, margin, width-2*margin, height-2*margin);
  float y = map(0, -20, 20, 0, height-2*margin) + margin;
  line(margin, y, width-margin, y);
  
  y = map(-9.8, -20, 20, 0, height-2*margin) + margin;
  line(margin, y, width-margin, y);
  text("1 g", 15, y+15);
  
  y = map(9.8, -20, 20, 0, height-2*margin) + margin;
  line(margin, y, width-margin, y);
  text("-1 g", 15, y-5);
  
  //y = map(-2*9.8, -20, 20, 0, height-2*margin) + margin;
  //line(margin, y, width-margin, y);
  
  //y = map(2*9.8, -20, 20, 0, height-2*margin) + margin;
  //line(margin, y, width-margin, y);
  
  stroke(0x44);
  
  y = map(-9.8/2, -20, 20, 0, height-2*margin) + margin;
  line(margin, y, width-margin, y);
  
  y = map(9.8/2, -20, 20, 0, height-2*margin) + margin;
  line(margin, y, width-margin, y);
}

void DrawSamples()
{
  for(int j = 0; j < samples.length; j++)
  {
    for(int i = 1; i < sampleIndex; i++)
    {
      float[] sample = samples[j];
      float y1 = map(sample[i-1], 20, -20, 0, height-2*margin);
      float y2 = map(sample[i], 20, -20, 0, height-2*margin);
      
      float x1 = map(i-1, 0, maxSamples, 0, width-2*margin);
      float x2 = map(i, 0, maxSamples, 0, width-2*margin);
      
      stroke(colors[j]);
      line(x1+margin, y1+margin, x2+margin, y2+margin);
    }
  }
}

void serialEvent(Serial p) 
{
  String inString = p.readStringUntil(lf);
  if(inString != null)
  {
    inString = trim(inString);
    String[] tokens = split(inString, ';');
    if(tokens.length < 3)
      return;
    
    boolean isFull = sampleIndex == maxSamples-1;
    if(isFull)
      moveSamples();

    samples[0][sampleIndex] = float(tokens[0]);//-0.88
    samples[1][sampleIndex] = float(tokens[1]);
    samples[2][sampleIndex] = float(tokens[2]);//-0.7
    
    if(!isFull)
      sampleIndex += 1;
  }
} 

void moveSamples()
{
  for (int i = 0; i < samples.length; i++)
  {
      for (int j1 = 0, j2 = 1; j2 < samples[i].length; j1++, j2++)
        samples[i][j1] = samples[i][j2];
  }
}

