# Unity-Raspberry-TCP-connection-test
 This is a simple example of TCP socket communication between Unity 2020.1.01f1 Android (client) and Raspberry pi 4 model B (server). In this case the client can turn on and off a LED connected to the Raspberry and see its state (on or off) from the UI. This is just an example to be able to build bigger things, the LED could be replaced by an heptic actuator for example.
 
 The .py file must be run on the Raspberry with the command "sudo python3 tcpSocketServer.py" being on the same directory as the document. The Unity Project should be built for Android and the resulting .apk must be run on an Android device. Make sure that the IP of the server on the TCPListener object from the example scene is your Raspberry's static IP.
 
 A low quality .gif of the example working:
 
 
![exampleWorking2](https://user-images.githubusercontent.com/47749352/111707124-cab55580-8843-11eb-9239-1149efec7492.gif)

 
 
# Raspberry pi setup
First of all you will need to install the Raspbian (or similar) operative system on an SD card. For that, connect the SD card to your computer, install the Raspberry Pi Imager: https://www.raspberrypi.org/software/ and follow these 40 second video for installing the OS on your SD card: https://youtu.be/J024soVgEeM.

Once you have done that you can plug it in your Raspberry pi, check you have ssh installed in your computer (if you are on windows you can follow this tutorial https://jcutrer.com/windows/install-openssh-on-windows10 and make sure the executable's route is included in your environtment variables), connect an ethernet cable to your Raspberry pi and your PC and power on the board. Then type on the command line "ssh pi@raspberrypi". If that doesn't work try typing "arp -a" and finding the Raspberry's ip, once you find it connect to it by typing *ssh pi@xxxxxxxxxx* (where the Xs are the IP numbers). If that doesn't work try googling or contact me :D 

The next step is to setup a static ip for the Raspberry pi, so it won't change each time you connect it to the router via Ethernet (you could also use wifi, but that won't be covered here). Here is a tutorial on how to set up a static ip on the board: https://pimylifeup.com/raspberry-pi-static-ip-address/#:~:text=First%2C%20you%20have%20to%20decide,assign%20to%20your%20Raspberry%20Pi. 
Then you will be able to connect to the raspberry with the terminal writing *ssh pi@xxxxxx* (where the Xs are the static IP address). Be careful with the firewall!

As a side note make sure your raspberry pi has python3 installed and remember that most problems on linux can be solved with an apt-get install x :^)

Also, for transfering files from your computer to the raspberry and viceversa, I recommend FileZilla: https://filezilla-project.org

Hopefully I mentioned everything!

# LED circuit setup
This one is easy: you will only need a red or green LED, a 330 ohm (220 works well as well, a bit more shiny) resitor, a breadboard and your Raspberry. You should connect your GPIO pin 18 to the resistor, the resistor to the LED (make sure the LED's polarity is the right one, you have 50/50 chance if you put it randomly) and the other LED's end to the Raspberry's ground, as shown on the figure:

![image](https://user-images.githubusercontent.com/47749352/111705980-cee07380-8841-11eb-9598-831682a04677.png)

And basically that's all the setup you will need to make this little project (which can be the foundation to bigger ones) work.
Good luck and happy coding!
