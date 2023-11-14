from statistics import mean 

import globals_vars as gv

valueToDetectMovement = {
    "left": {
        "leftHand": {
            "x": [0, 1/3],
            "y": [1/3, 1]
        },
        "rightHand": {
            "x": [0, 1/3], 
            "y": [0, 2/3] 
        },
    },
    "right": {
        "leftHand": {
            "x": [2/3, 1],
            "y": [0, 2/3]
        },
        "rightHand": {
            "x": [2/3, 1],
            "y": [1/3, 1]
        },
    },
    "top": {
        "leftHand": {
            "x": [0, 2/3],
            "y": [0, 1/3]
        },
        "rightHand": {
            "x": [1/3, 1], 
            "y": [0, 1/3] 
        },
    },
    "bottom": {
        "leftHand": {
            "x": [0, 2/3],
            "y": [2/3, 1]
        },
        "rightHand": {
            "x": [1/3, 1], 
            "y": [2/3, 1]
        },
    },
}

nose_y_points = [] # Use for avg value of nose

def getCorner(handPosRight, handPosLeft):
    is_top_left = False
    is_top_right = False
    is_bot_left = False
    is_bot_right = False
    is_center = False

    if isInSquare(handPosLeft, gv.TOP_LEFT_CORNER) or isInSquare(handPosRight, gv.TOP_LEFT_CORNER):
        is_top_left = True
    if isInSquare(handPosLeft, gv.TOP_RIGHT_CORNER) or isInSquare(handPosRight, gv.TOP_RIGHT_CORNER):
        is_top_right = True
    if isInSquare(handPosLeft, gv.BOT_LEFT_CORNER) or isInSquare(handPosRight, gv.BOT_LEFT_CORNER):
        is_bot_left = True
    if isInSquare(handPosLeft, gv.BOT_RIGHT_CORNER) or isInSquare(handPosRight, gv.BOT_RIGHT_CORNER):
        is_bot_right = True
    if isInSquare(handPosLeft, gv.CENTER) or isInSquare(handPosRight, gv.CENTER):
        is_center = True
    return is_top_left, is_top_right, is_bot_left, is_bot_right, is_center

def isInSquare(point, square):
     if square[0][0] <= point[0] <= square[1][0] and square[0][1] <= point[1] <= square[1][1]:
         return True
     return False

def getMovementType(handPosRight, handPosLeft):
    handXRight, handYRight = handPosRight
    handXLeft, handYLeft = handPosLeft

    for movement_type, movement_details in valueToDetectMovement.items(): # Récupération des positions de chaque main pour chaque pose
        leftHandXRange = movement_details["leftHand"]["x"]
        leftHandYRange = movement_details["leftHand"]["y"]
        rightHandXRange = movement_details["rightHand"]["x"]
        rightHandYRange = movement_details["rightHand"]["y"]

        # Réarrangement par rapport à la taille de l'écran
        leftHandXRange = [leftHandXRange[0] * gv.WIDTH, leftHandXRange[1] * gv.WIDTH]
        leftHandYRange = [leftHandYRange[0] * gv.HEIGHT, leftHandYRange[1] * gv.HEIGHT]
        rightHandXRange = [rightHandXRange[0] * gv.WIDTH, rightHandXRange[1] * gv.WIDTH]
        rightHandYRange = [rightHandYRange[0] * gv.HEIGHT, rightHandYRange[1] * gv.HEIGHT]

        # Si les main sont dans une certaine pose alors on retourne la pose
        if (leftHandXRange[0] <= handXLeft <= leftHandXRange[1] and
            leftHandYRange[0] <= handYLeft <= leftHandYRange[1] and
            rightHandXRange[0] <= handXRight <= rightHandXRange[1] and
            rightHandYRange[0] <= handYRight <= rightHandYRange[1]):
            return movement_type
        
    return "None"

def isJump(lmList):
    if len(nose_y_points) > 0:
        try:
            nose_y = lmList[gv.NOSE][2]
            avg_y_points = mean(nose_y_points)
            if nose_y < avg_y_points - gv.JUMP_RANGE:
                return True
        except Exception as e:
            print(e)
    return False

def isSquat(lmList):
    try:
        left_hip_y = lmList[gv.LEFT_HIP][2]
        left_knee_y = lmList[gv.LEFT_KNEE][2]
        left_ankle_y = lmList[gv.LEFT_ANKLE][2]
        # print(f"hip: {left_hip_y} // knee: {left_knee_y} // ankle: {left_ankle_y}")
        if left_hip_y >= left_knee_y - gv.SQUAT_RANGE and left_knee_y >= left_ankle_y - gv.SQUAT_RANGE:
            return True
    except Exception as e:
        print(e)
    return False

def add_y_point(lmList):
    try:
        nose_y_points.append(lmList[gv.NOSE][2])
    except Exception as e:
        print(e)
