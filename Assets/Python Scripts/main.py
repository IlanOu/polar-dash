import cv2
import time
import PoseModule
import mediapipe as mp
import numpy as np
import random
import UdpComms as U
import poseDetection as pd
import globals_vars as gv

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

def coliderCircle(centerPos, radius, handPos):
    centerX, centerY = centerPos
    handX, handY = handPos

    # Utilisez la distance euclidienne pour vérifier si le point est à l'intérieur du cercle
    distance = ((handX - centerX) ** 2 + (handY - centerY) ** 2) ** 0.5

    if distance <= radius:
        return True
    else:
        return False

def getRightPointPosition(lmList, point):

    try:
        xPos = lmList[point][1]
        yPos = lmList[point][2]
    except:
        xPos = 0
        yPos = 0
    return (xPos, yPos)

def printRect(img, first_point, second_point):
    first_x, first_y = first_point
    second_x, second_y = second_point
    cv2.rectangle(img, (first_x, first_y), (second_x, second_y), gv.RED, 2)

def default_view():
    text_top_left_corner = (gv.TOP_LEFT_CORNER[0][0], gv.TOP_LEFT_CORNER[0][1] + gv.CARACTER_HEIGHT)
    text_top_right_corner = (gv.TOP_RIGHT_CORNER[0][0], gv.TOP_RIGHT_CORNER[0][1] + gv.CARACTER_HEIGHT)
    text_bot_left_corner = (gv.BOT_LEFT_CORNER[0][0], gv.BOT_LEFT_CORNER[0][1] + gv.CARACTER_HEIGHT)
    text_bot_right_corner = (gv.BOT_RIGHT_CORNER[0][0], gv.BOT_RIGHT_CORNER[0][1] + gv.CARACTER_HEIGHT)
    text_center = (gv.CENTER[0][0], gv.CENTER[0][1] + gv.CARACTER_HEIGHT)
    if is_top_left:
        cv2.putText(img, f"TOP LEFT", text_top_left_corner, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
    if is_top_right:
        cv2.putText(img, f"TOP RIGHT", text_top_right_corner, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
    if is_bot_left:
        cv2.putText(img, f"BOT LEFT", text_bot_left_corner, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
    if is_bot_right:
        cv2.putText(img, f"BOT RIGHT", text_bot_right_corner, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
    if is_center:
        cv2.putText(img, f"CENTER", text_center, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
        
    printRect(img, gv.TOP_LEFT_CORNER[0], gv.TOP_LEFT_CORNER[1])
    printRect(img, gv.TOP_RIGHT_CORNER[0], gv.TOP_RIGHT_CORNER[1])
    printRect(img, gv.BOT_LEFT_CORNER[0], gv.BOT_LEFT_CORNER[1])
    printRect(img, gv.BOT_RIGHT_CORNER[0], gv.BOT_RIGHT_CORNER[1])
    printRect(img, gv.CENTER[0], gv.CENTER[1])


cap = cv2.VideoCapture(0)
pTime = 0
detector = PoseModule.poseDetector()
screenSize = (gv.WIDTH, gv.HEIGHT)
# screenSize = (1920, 1080)

# & Variables

counter = 0
counterLimit = 10000

while True:

    counter += 1
    if counter % 4 ==  0:
        success, img = cap.read()
        
        if not success:
            break
        
        img = cv2.resize(img, screenSize)
        img = cv2.flip(img,1)
        
        img = detector.findPose(img, gv.SHOW_BONES)
        lmList = detector.getPosition(img, False)

        try:
            length = lmList[19][2]
        except Exception as e:
            length = 0
            print(e)
            

        

        #& ----------------------------- MES FONCTIONS -----------------------------

        handPosRight = getRightPointPosition(lmList, gv.RIGHT_INDEX)    
        handPosLeft = getRightPointPosition(lmList, gv.LEFT_INDEX)

        # JUMP
        jump = False
        if counter > 500:
            if pd.isJump(lmList):
                jump = True
        else:
            pd.add_y_point(lmList)

        # CORNER
        is_top_left, is_top_right, is_bot_left, is_bot_right, is_center = pd.getCorner(handPosRight, handPosLeft)

        # OLD DEFINE MOVEMENT
        movementType = pd.getMovementType(handPosRight, handPosLeft)

        # SQUAT
        squat = pd.isSquat(lmList)
                
        #& --------------------------- PAS MES FONCTIONS ----------------------------



        cTime = time.time()
        fps = 1 / (cTime - pTime)
        pTime = cTime

        # sock.SendData(movementType) # Send this string to other application
        # if movementType:
        #     cv2.putText(img, f"Type de mouvement : {movementType}", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)
        
        # default_view()

        if squat:
            cv2.putText(img, f"SQUAT", (int(gv.WIDTH/2 - (gv.CARACTER_WIDTH*5/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)

        # if jump:
        #     cv2.putText(img, f"SAUTE", (int(gv.WIDTH/2 - (gv.CARACTER_WIDTH*5/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)


        cv2.imshow("Webcam", img)
        cv2.waitKey(1)

    if counter >= counterLimit :
        counter = 0