from statistics import mean 
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
# region default_view
# def default_view():
#     text_top_left_corner = (gv.TOP_LEFT_CORNER[0][0], gv.TOP_LEFT_CORNER[0][1] + gv.CARACTER_HEIGHT)
#     text_top_right_corner = (gv.TOP_RIGHT_CORNER[0][0], gv.TOP_RIGHT_CORNER[0][1] + gv.CARACTER_HEIGHT)
#     text_bot_left_corner = (gv.BOT_LEFT_CORNER[0][0], gv.BOT_LEFT_CORNER[0][1] + gv.CARACTER_HEIGHT)
#     text_bot_right_corner = (gv.BOT_RIGHT_CORNER[0][0], gv.BOT_RIGHT_CORNER[0][1] + gv.CARACTER_HEIGHT)
#     text_center = (gv.CENTER[0][0], gv.CENTER[0][1] + gv.CARACTER_HEIGHT)
#     if is_top_left:
#         cv2.putText(img, f"TOP LEFT", text_top_left_corner, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
#     if is_top_right:
#         cv2.putText(img, f"TOP RIGHT", text_top_right_corner, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
#     if is_bot_left:
#         cv2.putText(img, f"BOT LEFT", text_bot_left_corner, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
#     if is_bot_right:
#         cv2.putText(img, f"BOT RIGHT", text_bot_right_corner, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
#     if is_center:
#         cv2.putText(img, f"CENTER", text_center, cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
        
#     printRect(img, gv.TOP_LEFT_CORNER[0], gv.TOP_LEFT_CORNER[1])
#     printRect(img, gv.TOP_RIGHT_CORNER[0], gv.TOP_RIGHT_CORNER[1])
#     printRect(img, gv.BOT_LEFT_CORNER[0], gv.BOT_LEFT_CORNER[1])
#     printRect(img, gv.BOT_RIGHT_CORNER[0], gv.BOT_RIGHT_CORNER[1])
#     printRect(img, gv.CENTER[0], gv.CENTER[1])
# endregion default_view

def verif_analyse():
    global start_time, time_to_move
    if not time_to_move and time.time() >= start_time + gv.ANALYSE_DURATION:
        time_to_move = True
        return True
    return False

def auto_config(left_femurSizes, right_femurSizes):
    try:
        gv.LEFT_SQUAT_RANGE = mean(left_femurSizes) / gv.COEF_SQUAT_RANGE
        gv.LEFT_JUMP_RANGE = mean(left_femurSizes) / gv.COEF_JUMP_RANGE
    except Exception as e:
        print("Erreur analyse")


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
time_to_move = False
start_time = time.time()

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
        left_poseDetection.print_active = False
        left_poseDetection.refreshPose(left_lmList)

        right_img = right_detector.findPose(right_img, gv.SHOW_BONES)
        right_lmList = right_detector.getPosition(right_img, False)
        right_poseDetection.print_active = False
        right_poseDetection.refreshPose(right_lmList) 
        

        

        #& ----------------------------- MES FONCTIONS -----------------------------

        # AUTO CONFIG
        left_femurSizes = []
        right_femurSizes = []
        if not time_to_move:
            # Adjust range
            left_femurSizes.append(left_poseDetection.auto_config())
            right_femurSizes.append(right_poseDetection.auto_config())


            # Set values to detect jump
            left_poseDetection.add_y_point()
            right_poseDetection.add_y_point()

            if verif_analyse():
                auto_config(left_femurSizes, right_femurSizes)
        else:
            # JUMP
            left_poseDetection.isJump(gv.LEFT_JUMP_RANGE)
            right_poseDetection.isJump(gv.RIGHT_JUMP_RANGE)
            
            # SQUAT
            left_poseDetection.isSquat(gv.LEFT_SQUAT_RANGE)
            right_poseDetection.isSquat(gv.RIGHT_SQUAT_RANGE)


        # OLD DEFINE MOVEMENT
        # movementType = pd.getMovementType(handPosRight, handPosLeft)

                        
        #& --------------------------- PAS MES FONCTIONS ----------------------------

        # sock.SendData(movementType) # Send this string to other application
        # if movementType:
        #     cv2.putText(img, f"Type de mouvement : {movementType}", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)

        # region Affichage des résultats

        print_squat = True
        if print_squat:
            msg = "SQUAT"
            if left_poseDetection.squat:
                if not left_poseDetection.old_squat:
                    sock.SendData("left:squat")
                cv2.putText(left_img, msg, (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            if right_poseDetection.squat:
                if not right_poseDetection.old_squat:
                    sock.SendData("right:squat")
                cv2.putText(right_img, msg, (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)

        print_saut = True
        if print_saut:
            msg = "SAUTE"
            if left_poseDetection.jump:
                if not left_poseDetection.old_jump:
                    sock.SendData("left:jump")
                cv2.putText(left_img, msg, (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.GREEN, 2)
            if right_poseDetection.jump:
                if not right_poseDetection.old_jump:
                    sock.SendData("right:jump")
                cv2.putText(right_img, msg, (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.GREEN, 2)

        if left_poseDetection.here and not left_poseDetection.old_here:
            sock.SendData("left:here")
        elif not left_poseDetection.here and left_poseDetection.old_here:
            sock.SendData("left:nothere")
        if right_poseDetection.here and not right_poseDetection.old_here:
            sock.SendData("right:here")
        elif not right_poseDetection.here and right_poseDetection.old_here:
            sock.SendData("right:nothere")

        left_poseDetection.refreshOldValue()
        right_poseDetection.refreshOldValue()

        if not time_to_move:
            msg = "ATTENDEZ LORS DE L'ANALYSE"
            cv2.putText(img, msg, (int(gv.WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)

        # endregion Affichage des résultats

        cv2.imshow("Webcam", img)
        cv2.waitKey(1)

    if counter >= counterLimit :
        counter = 0