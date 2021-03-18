#---------------------------------------------------------------------------------------
# This script starts a simple tcp server on the Raspberry Pi board, which connects to 
# the Android Unity client and allows it to turn on and off a LED connected to the  board
# (this LED could represent a haptic sensor controlled by the Raspberry)
#--------------
# Victor Blanco Bataller
# 2021/03/18
# Turku University of Applied Sciences
#----------------------------------------------------------------------------------------


from threading import Thread
import socket
import time
import RPi.GPIO as GPIO

# Make this True if you want to print more details on the Raspberry cmd while debugging
VERBOSE = False

# port where the server will be allocated
IP_PORT = 50000

# pin where the LED is connected
P_BUTTON = 18 # adapt to your wiring

#-------------------------------
# setup()
# Function to set up the pin for the LED
def setup():

    # We set up our pin (18 in our case) so it can work as an output pin and power the LED
    GPIO.setmode(GPIO.BCM)
    GPIO.setwarnings(False)
    GPIO.setup(P_BUTTON, GPIO.OUT)

#-------------------------------
# debug()
# Function used for getting more details while debugging
def debug(text):
    if VERBOSE:
        print("Debug:---", text)


#.....................................................................
# ---------------------- class SocketHandler ------------------------
#---------------------------------------------------------------------
class SocketHandler(Thread):
    def __init__(self, conn):
        Thread.__init__(self)
        self.conn = conn

    #---------------------------
    # run():
    # This function is the one that listens to the client, waiting for some message so it can send it to executeCommand()
    # if there are no messages it just iterates without doing anything
    #-----------------------------
    def run(self):
        global isConnected
        debug("SocketHandler started")
        while True:
            cmd = ""
            try:
                debug("Calling blocking conn.recv()")
                cmd = str(self.conn.recv(1024).decode('utf-8'))
            except:
                debug("exception in conn.recv()") 
                print("Connection reset from the peer")
                # happens when connection is reset from the peer
                break
            #debug("Received cmd: " + cmd + " len: " + str(len(cmd)))
            if len(cmd) == 0:
                break
            

            # We call the function which determines what happens to the LED depending on the message
            self.executeCommand(cmd)
        conn.close()
        print ("Client disconnected. Waiting for next client...")
        isConnected = False
        debug("SocketHandler terminated")

    #---------
    # executeCommand(cmd):
    # Called when the server recieves a message, turns LED on if message is "on" and off if it is "off".
    # Also returns the message to the client, so it can display it somewhere
    #--------
    def executeCommand(self, cmd):
        #debug("Calling executeCommand() with  cmd: " + cmd)


        # Now we send the data to the client
        self.conn.send(cmd.encode('utf-8'))

        if str(cmd[:-2]) == 'on':  # remove new line characters

            # turn LED on
            GPIO.output(P_BUTTON,GPIO.HIGH)

            print ("Reporting current state:", "LED ON")

        elif str(cmd[:-2]) == 'off': 

            # turn led off
            GPIO.output(P_BUTTON,GPIO.LOW)
            
            print ("Reporting current state:", "LED OFF")

            
        else:

            
            print ("Reporting current state:", str(cmd[:-2]))


#---------------------------------------------------------------           
# ----------------- End of SocketHandler -----------------------
#---------------------------------------------------------------

# Now that we have defined what we need, it is time to execute everything:

#----------------------------------------------------------------
#           Invocation of the functions, execution:
#---------------------------------------------------------------

# set up the pin
setup()

serverSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
# close port when process exits:
serverSocket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1) 

debug("Socket created")

HOSTNAME = "" # Symbolic name meaning all available interfaces


# try to create the server and print an error message and exit if that fails 
try:

    serverSocket.bind((HOSTNAME, IP_PORT))

except socket.error as msg:

    print ("Bind failed", msg[0], msg[1])
    sys.exit()

# now the server waits for clients
serverSocket.listen(10)

print ("Waiting for a connecting client...")

# not connected to a client
isConnected = False

#-----------------------------------------------------------------------
# this loop is what allows the server to keep looking for clients once the first one is disconnected
while True:

    debug("Calling blocking accept()...")

    # we create the connection and the client's address once the connection is accepted
    conn, addr = serverSocket.accept()

    print("Connected with client at " + addr[0])

    # now we are connected
    isConnected = True

    # we create our socket handler:
    socketHandler = SocketHandler(conn)

    # necessary to terminate it at program termination:
    socketHandler.setDaemon(True)  

    # start our socket handler (the one which will recieve the messages and act upon them)
    socketHandler.start()

    t = 0
    # dummy loop just to remind us that the server is connected
    while isConnected:
        print("Server connected at", t, "s")
        time.sleep(10)
        t += 10

#---------------------------------------------------------------------------------
# End of execution code
#--------------------------------------------------------------------------------
