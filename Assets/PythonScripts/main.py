from statistics import mean 
import cv2
import time
import UdpComms as U
from poseDetection import PoseDetection as pd
import globals_vars as gv
import saveImage as si
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

def auto_config(left_shouldersSize, right_shouldersSize):
    try:
        gv.LEFT_SQUAT_RANGE = int(left_shouldersSize / gv.COEF_SQUAT_RANGE)
        gv.LEFT_JUMP_RANGE = int(left_shouldersSize / gv.COEF_JUMP_RANGE)
        gv.LEFT_RANGE_TOP_SQUARE = int(left_shouldersSize / gv.COEF_TOP_RANGE)
        gv.LEFT_RANGE_BOTTOM_SQUARE = int(left_shouldersSize / gv.COEF_BOTTOM_RANGE)
        gv.LEFT_RANGE_LEFT_SQUARE = int(left_shouldersSize / gv.COEF_LEFT_RANGE)
        gv.LEFT_RANGE_RIGHT_SQUARE = int(left_shouldersSize / gv.COEF_RIGHT_RANGE)
        
        if not gv.IS_SOLO_PLAYER:
            gv.RIGHT_SQUAT_RANGE = int(right_shouldersSize / gv.COEF_SQUAT_RANGE)
            gv.RIGHT_JUMP_RANGE = int(right_shouldersSize / gv.COEF_JUMP_RANGE)
            gv.RIGHT_RANGE_TOP_SQUARE = int(right_shouldersSize / gv.COEF_TOP_RANGE)
            gv.RIGHT_RANGE_BOTTOM_SQUARE = int(right_shouldersSize / gv.COEF_BOTTOM_RANGE)
            gv.RIGHT_RANGE_LEFT_SQUARE = int(right_shouldersSize / gv.COEF_LEFT_RANGE)
            gv.RIGHT_RANGE_RIGHT_SQUARE = int(right_shouldersSize / gv.COEF_RIGHT_RANGE)
        else:
            gv.RIGHT_SQUAT_RANGE = gv.LEFT_SQUAT_RANGE
            gv.RIGHT_JUMP_RANGE = gv.LEFT_JUMP_RANGE
            gv.RIGHT_RANGE_TOP_SQUARE = gv.LEFT_RANGE_TOP_SQUARE
            gv.RIGHT_RANGE_BOTTOM_SQUARE = gv.LEFT_RANGE_BOTTOM_SQUARE
            gv.RIGHT_RANGE_LEFT_SQUARE = gv.LEFT_RANGE_LEFT_SQUARE
            gv.RIGHT_RANGE_RIGHT_SQUARE = gv.LEFT_RANGE_RIGHT_SQUARE
    except Exception as e:
        print("Erreur analyse")
        #print(e)


cap = cv2.VideoCapture(0)
pTime = 0
screenSize = (gv.WIDTH, gv.HEIGHT)
left_detector = PoseModule.poseDetector()
right_detector = PoseModule.poseDetector()

left_poseDetection = pd()
left_poseDetection.setSide("LEFT")
right_poseDetection = pd()
right_poseDetection.setSide("RIGHT")

# & Variables
counter = 0
counterLimit = 10000
time_to_move = True
start_time = time.time()
getImage = False
photo = None

while True:
    counter += 1
    received_data = sock.ReadReceivedData()
    if received_data != None:
        getImage = True
        
    if counter % 2 ==  0:
        success, img = cap.read()
        if not success:
            break
        
        # region Image
        img = cv2.resize(img, screenSize)
        img = cv2.flip(img,1)

        image_name = None
        if getImage:
            getImage = False
            photo = img
            image_name = si.compress_and_save_image(photo)

        cv2.line(img, (gv.SCREEN_SEPARATOR, 0), (gv.SCREEN_SEPARATOR, gv.HEIGHT), gv.BLACK, 2)

        # Screen separation
        left_img = img[:, :gv.SCREEN_SEPARATOR]
        right_img = img[:, gv.SCREEN_SEPARATOR:]

        # endregion Image

        # region Get Pose

        # Get new position
        left_img = left_detector.findPose(left_img, gv.SHOW_BONES)
        left_lmList = left_detector.getPosition(left_img, False)
        left_poseDetection.print_active = False
        left_poseDetection.refreshPose(left_lmList)

        right_img = right_detector.findPose(right_img, gv.SHOW_BONES)
        right_lmList = right_detector.getPosition(right_img, False)
        right_poseDetection.print_active = False
        right_poseDetection.refreshPose(right_lmList) 
        
        # endregion Get Pose

        # region Calcul

        # ANALYSE POINTS
        if not time_to_move:
            # Set values to detect jump
            # left_poseDetection.add_y_point()
            # right_poseDetection.add_y_point()

            # check if analyse end
            # verif_analyse()
            temp = ""
        # MAIN LOOP
        else:
            # AUTO CONFIG
            left_femurSize = left_poseDetection.auto_config(start_time)
            right_femurSize = right_poseDetection.auto_config(start_time)
            auto_config(left_femurSize, right_femurSize)

            # JUMP
            left_poseDetection.isJump(gv.LEFT_JUMP_RANGE)
            right_poseDetection.isJump(gv.RIGHT_JUMP_RANGE)
            # if left_poseDetection.default_y_nose != None:
            #     cv2.line(left_img, (0, left_poseDetection.default_y_nose - gv.LEFT_JUMP_RANGE), (gv.LEFT_WIDTH, left_poseDetection.default_y_nose - gv.LEFT_JUMP_RANGE), gv.RED, 2)
            
            # SQUAT
            left_poseDetection.isSquat(gv.LEFT_SQUAT_RANGE)
            right_poseDetection.isSquat(gv.RIGHT_SQUAT_RANGE)

            # ARMS POS
            left_center = left_poseDetection.resetArea()
            right_center = right_poseDetection.resetArea()

            if left_center:
                cv2.circle(left_img, left_center, 10, gv.GREEN, 2)
            if right_center:
                cv2.circle(right_img, right_center, 10, gv.GREEN, 2)

            left_poseDetection.arms_detection()
            right_poseDetection.arms_detection()

        # endregion Calcul

        # region Affichage Debug

        # printRect(left_img, gv.TOP_AREA[0][0], gv.TOP_AREA[0][1])
        # printRect(left_img, gv.BOTTOM_AREA[0][0], gv.BOTTOM_AREA[0][1])
        # printRect(left_img, gv.LEFT_AREA[0][0], gv.LEFT_AREA[0][1])
        # printRect(left_img, gv.RIGHT_AREA[0][0], gv.RIGHT_AREA[0][1])

        # printRect(right_img, gv.TOP_AREA[1][0], gv.TOP_AREA[1][1])
        # printRect(right_img, gv.BOTTOM_AREA[1][0], gv.BOTTOM_AREA[1][1])
        # printRect(right_img, gv.LEFT_AREA[1][0], gv.LEFT_AREA[1][1])
        # printRect(right_img, gv.RIGHT_AREA[1][0], gv.RIGHT_AREA[1][1])

        # endregion Affichage Debug

        # region Résultats

        active_squat = True
        if active_squat:
            msg = "SQUAT"
            if left_poseDetection.squat and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT")):
                if not left_poseDetection.old_squat:
                    sock.SendData("left:squat")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT":
                        sock.SendData("right:squat")
                cv2.putText(left_img, msg, (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            if right_poseDetection.squat and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT")):
                if not right_poseDetection.old_squat:
                    sock.SendData("right:squat")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT":
                        sock.SendData("left:squat")
                cv2.putText(right_img, msg, (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)

        active_jump = True
        if active_jump:
            msg = "SAUTE"
            if left_poseDetection.jump and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT")):
                if not left_poseDetection.old_jump:
                    sock.SendData("left:jump")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT":
                        sock.SendData("right:jump")
                cv2.putText(left_img, msg, (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.GREEN, 2)
            if right_poseDetection.jump and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT")):
                if not right_poseDetection.old_jump:
                    sock.SendData("right:jump")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT":
                        sock.SendData("left:jump")
                cv2.putText(right_img, msg, (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.GREEN, 2)

        active_arms_pos = True
        if active_arms_pos:
            msg = "TOP"
            if left_poseDetection.arms_top and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT")):
                if not left_poseDetection.old_arms_top:
                    sock.SendData("left:top")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT":
                        sock.SendData("right:top")
                cv2.putText(left_img, msg, (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            if right_poseDetection.arms_top and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT")):
                if not right_poseDetection.old_arms_top:
                    sock.SendData("right:top")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT":
                        sock.SendData("left:top")
                cv2.putText(right_img, msg, (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            msg = "BOTTOM"
            if left_poseDetection.arms_bottom and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT")):
                if not left_poseDetection.old_arms_bottom:
                    sock.SendData("left:bottom")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT":
                        sock.SendData("right:bottom")
                cv2.putText(left_img, msg, (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            if right_poseDetection.arms_bottom and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT")):
                if not right_poseDetection.old_arms_bottom:
                    sock.SendData("right:bottom")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT":
                        sock.SendData("left:bottom")
                cv2.putText(right_img, msg, (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            
            msg = "SIDE"
            if left_poseDetection.arms_side and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT")):
                if not left_poseDetection.old_arms_side:
                    sock.SendData("left:side")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT":
                        sock.SendData("right:side")
                cv2.putText(left_img, msg, (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            if right_poseDetection.arms_side and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT")):
                if not right_poseDetection.old_arms_side:
                    sock.SendData("right:side")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT":
                        sock.SendData("left:side")
                cv2.putText(right_img, msg, (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)

            msg = "LEFT"
            if left_poseDetection.arms_left and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT")):
                if not left_poseDetection.old_arms_left:
                    sock.SendData("left:left")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT":
                        sock.SendData("right:left")
                cv2.putText(left_img, msg, (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            if right_poseDetection.arms_left and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT")):
                if not right_poseDetection.old_arms_left:
                    sock.SendData("right:left")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT":
                        sock.SendData("left:left")
                cv2.putText(right_img, msg, (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            msg = "RIGHT"
            if left_poseDetection.arms_right and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT")):
                if not left_poseDetection.old_arms_right:
                    sock.SendData("left:right")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "LEFT":
                        sock.SendData("right:right")
                cv2.putText(left_img, msg, (int(gv.LEFT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)
            if right_poseDetection.arms_right and (not gv.IS_SOLO_PLAYER or (gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT")):
                if not right_poseDetection.old_arms_right:
                    sock.SendData("right:right")
                    if gv.IS_SOLO_PLAYER and gv.SOLO_PLAYER == "RIGHT":
                        sock.SendData("left:right")
                cv2.putText(right_img, msg, (int(gv.RIGHT_WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)


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

        if image_name != None:
            sock.SendData(f"image:{image_name}")

        if not time_to_move:
            msg = "ATTENDEZ LORS DE L'ANALYSE"
            cv2.putText(img, msg, (int(gv.WIDTH/2 - (gv.CARACTER_WIDTH*len(msg)/2)), 0 + gv.CARACTER_HEIGHT), cv2.FONT_HERSHEY_SIMPLEX, 1, gv.RED, 2)

        # endregion Résultats

        cv2.imshow("Webcam", img)
        cv2.waitKey(1)

    if counter >= counterLimit :
        counter = 0