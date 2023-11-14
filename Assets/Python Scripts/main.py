import cv2
import time
import UdpComms as U
from poseDetection import PoseDetection as pd
import globals_vars as gv
import PoseModule

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

def printRect(img, first_point, second_point):
    first_x, first_y = first_point
    second_x, second_y = second_point
    cv2.rectangle(img, (first_x, first_y), (second_x, second_y), gv.RED, 2)

# A voir si on garde
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

def detection_jump(poseDetection):
    global time_to_jump
    if counter > 200:
        if not time_to_jump:
            time_to_jump = True
        poseDetection.isJump()
    else:
        poseDetection.add_y_point()



cap = cv2.VideoCapture(0)
pTime = 0
screenSize = (gv.WIDTH, gv.HEIGHT)
left_detector = PoseModule.poseDetector()
right_detector = PoseModule.poseDetector()

left_poseDetection = pd()
right_poseDetection = pd()

# & Variables
counter = 0
counterLimit = 10000
time_to_jump = False


while True:
    counter += 1
    if counter % 2 ==  0:
        success, img = cap.read()
        if not success:
            break
        
        img = cv2.resize(img, screenSize)
        img = cv2.flip(img,1)

        cv2.line(img, (gv.SCREEN_SEPARATOR, 0), (gv.SCREEN_SEPARATOR, gv.HEIGHT), gv.BLACK, 2)

        # Screen separation
        left_img = img[:, :gv.SCREEN_SEPARATOR]
        right_img = img[:, gv.SCREEN_SEPARATOR:]

        # Get new position
        left_img = left_detector.findPose(left_img, gv.SHOW_BONES)
        left_lmList = left_detector.getPosition(left_img, False)
        left_poseDetection.refreshPose(left_lmList)

        right_img = right_detector.findPose(right_img, gv.SHOW_BONES)
        right_lmList = right_detector.getPosition(right_img, False)
        right_poseDetection.refreshPose(right_lmList) 
        try:
            length = left_lmList[19][2]
            length = right_lmList[19][2]
        except Exception as e:
            length = 0
            print(e)

        

        #& ----------------------------- MES FONCTIONS -----------------------------

        # handPosRight = getRightPointPosition(lmList, gv.RIGHT_INDEX)    
        # handPosLeft = getRightPointPosition(lmList, gv.LEFT_INDEX)

        # JUMP
        detection_jump(left_poseDetection)
        detection_jump(right_poseDetection)

        # CORNER
        # is_top_left, is_top_right, is_bot_left, is_bot_right, is_center = pd.getCorner(handPosRight, handPosLeft)

        # OLD DEFINE MOVEMENT
        # movementType = pd.getMovementType(handPosRight, handPosLeft)

        # SQUAT
        # squat = pd.isSquat(lmList)
                
        #& --------------------------- PAS MES FONCTIONS ----------------------------

        cTime = time.time()
        fps = 1 / (cTime - pTime)
        pTime = cTime

        # sock.SendData(movementType) # Send this string to other application
        # if movementType:
        #     cv2.putText(img, f"Type de mouvement : {movementType}", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)
        
        # default_view()

        # if squat:
        #     cv2.putText(img, f"SQUAT", (int(gv.WIDTH/2 - (gv.CARACTER_WIDTH*5/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)

        if left_poseDetection.jump:
            cv2.putText(left_img, f"SAUTE", (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*5/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.GREEN, 2)
        if right_poseDetection.jump:
            cv2.putText(right_img, f"SAUTE", (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*5/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.GREEN, 2)

        if not time_to_jump:
            cv2.putText(img, f"ATTENDEZ AVANT DE SAUTER", (int(gv.WIDTH/2 - (gv.CARACTER_WIDTH*24/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)


        cv2.imshow("Webcam", img)
        cv2.waitKey(1)

    if counter >= counterLimit :
        counter = 0