from statistics import mean 
import math
import globals_vars as gv
import PoseModule
import time

class PoseDetection:

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

    side = "None"
    print_active = True
    nose_y_points = [] # Use for avg value of nose
    default_y_nose = None
    analyse_duration = gv.ANALYSE_DURATION
    lmList = None
    here = False
    old_here = False
    jump = False
    old_jump = False
    jump_start = 0
    squat = False
    old_squat = False
    arms_top = False
    old_arms_top = False
    arms_bottom = False
    old_arms_bottom = False
    arms_side = False
    old_arms_side = False
    arms_left = False
    old_arms_left = False
    arms_right = False
    old_arms_right = False

    def setSide(self, side):
        self.side = side

    def refreshOldValue(self):
        self.old_here = self.here
        self.old_jump = self.jump
        self.old_squat = self.squat
        self.old_arms_top = self.arms_top
        self.old_arms_bottom = self.arms_bottom
        self.old_arms_left = self.arms_left
        self.old_arms_right = self.arms_right

    def calcul_distance(self, point1, point2):
        return math.sqrt((point1[1] - point2[1])**2 + (point1[2] - point2[2])**2)
    
    def auto_config(self, startTime):
        try:
            if not self.jump and time.time() >= startTime + self.analyse_duration:
                self.default_y_nose = self.lmList[gv.NOSE][2]
                self.analyse_duration += gv.ANALYSE_DURATION


            left_shoulder = self.lmList[gv.LEFT_SHOULDER]
            right_shoulder = self.lmList[gv.RIGHT_SHOULDER]
            shoulders_size = self.calcul_distance(left_shoulder, right_shoulder)
            return shoulders_size
        except Exception as e:
            if self.print_active:
                print(e)
        return None

    def refreshPose(self, lmList):
        # Try = test d'erreur 
        try:
            length = lmList[19][2]
            self.here = True
        except Exception as e:
            length = 0
            if self.print_active:
                print(e)
            self.here = False
        self.lmList = lmList

    def pointPosition(self, point):
        try:
            xPos = self.lmList[point][1]
            yPos = self.lmList[point][2]
        except:
            xPos = 0
            yPos = 0
        return (xPos, yPos)

    # A voir si on garde
    def getCorner(self, handPosRight, handPosLeft):
        is_top_left = False
        is_top_right = False
        is_bot_left = False
        is_bot_right = False
        is_center = False

        if self.isInSquare(handPosLeft, gv.TOP_LEFT_CORNER) or self.isInSquare(handPosRight, gv.TOP_LEFT_CORNER):
            is_top_left = True
        if self.isInSquare(handPosLeft, gv.TOP_RIGHT_CORNER) or self.isInSquare(handPosRight, gv.TOP_RIGHT_CORNER):
            is_top_right = True
        if self.isInSquare(handPosLeft, gv.BOT_LEFT_CORNER) or self.isInSquare(handPosRight, gv.BOT_LEFT_CORNER):
            is_bot_left = True
        if self.isInSquare(handPosLeft, gv.BOT_RIGHT_CORNER) or self.isInSquare(handPosRight, gv.BOT_RIGHT_CORNER):
            is_bot_right = True
        if self.isInSquare(handPosLeft, gv.CENTER) or self.isInSquare(handPosRight, gv.CENTER):
            is_center = True
        return is_top_left, is_top_right, is_bot_left, is_bot_right, is_center

    def isInSquare(self, point, square):
        if square[0][0] <= point[0] <= square[1][0] and square[0][1] <= point[1] <= square[1][1]:
            return True
        return False

    # A voir si on garde
    def getMovementType(self, handPosRight, handPosLeft):
        handXRight, handYRight = handPosRight
        handXLeft, handYLeft = handPosLeft

        for movement_type, movement_details in self.valueToDetectMovement.items(): # Récupération des positions de chaque main pour chaque pose
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

    # Ajoute au tableau le y du nez pour le saut
    def add_y_point(self):
            try:
                self.nose_y_points.append(self.lmList[gv.NOSE][2])
            except Exception as e:
                if self.print_active:
                    print(e)

    def checkNoErrorJump(self, jump_range):
        if time.time() >= self.jump_start + gv.ANALYSE_DURATION:
            try:
                nose_y = self.lmList[gv.NOSE][2]
                self.default_y_nose = nose_y - jump_range
            except Exception as e:
                if self.print_active:
                    print(e)

    def isJump(self, jump_range):
        if self.default_y_nose != None:
            try:
                nose_y = self.lmList[gv.NOSE][2]
                avg_y_points = self.default_y_nose
                if nose_y < avg_y_points - jump_range:
                    if not self.jump:
                        self.jump_start = time.time()
                    else:
                        self.checkNoErrorJump(jump_range)
                    self.jump = True
                    return
            except Exception as e:
                if self.print_active:
                    print(e)
        self.jump = False

    def isSquat(self, squat_range):
        try:
            left_hip_y = self.lmList[gv.LEFT_HIP][2]
            left_knee_y = self.lmList[gv.LEFT_KNEE][2]
            right_hip_y = self.lmList[gv.RIGHT_HIP][2]
            right_knee_y = self.lmList[gv.RIGHT_KNEE][2]
            if left_knee_y - left_hip_y < squat_range and right_knee_y - right_hip_y < squat_range:
                self.squat = True
                return
        except Exception as e:
            if self.print_active:
                print(e)
        self.squat = False
    
    def resetArea(self):
        try:
            x_points = [self.lmList[gv.LEFT_SHOULDER][1], self.lmList[gv.RIGHT_SHOULDER][1], self.lmList[gv.LEFT_HIP][1], self.lmList[gv.RIGHT_HIP][1]]
            y_points = [self.lmList[gv.LEFT_SHOULDER][2], self.lmList[gv.LEFT_HIP][2], self.lmList[gv.RIGHT_SHOULDER][2], self.lmList[gv.RIGHT_HIP][2]]
            center_x = int(mean(x_points))
            center_y = int(mean(y_points))
            # gv.AREA[LEFT/RIGHT][FIRST/SECOND POINT][X/Y]
            if self.side == "LEFT":
                gv.TOP_AREA[0][1][1] = center_y - gv.LEFT_RANGE_TOP_SQUARE
                gv.BOTTOM_AREA[0][0][1] = center_y + gv.LEFT_RANGE_BOTTOM_SQUARE
                gv.LEFT_AREA[0][1][0] = center_x - gv.LEFT_RANGE_LEFT_SQUARE
                gv.RIGHT_AREA[0][0][0] = center_x + gv.LEFT_RANGE_RIGHT_SQUARE

            if self.side == "RIGHT":
                gv.TOP_AREA[1][1][1] = center_y - gv.RIGHT_RANGE_TOP_SQUARE
                gv.BOTTOM_AREA[1][0][1] = center_y + gv.RIGHT_RANGE_BOTTOM_SQUARE
                gv.LEFT_AREA[1][1][0] = center_x - gv.RIGHT_RANGE_LEFT_SQUARE
                gv.RIGHT_AREA[1][0][0] = center_x + gv.RIGHT_RANGE_RIGHT_SQUARE

            return (center_x, center_y)
        except Exception as e:
            gv.resetSquare()
            if self.print_active:
                print(e)
        return None
    
    def arms_detection(self):
        try:
            left_hand = (self.lmList[gv.LEFT_WRIST][1], self.lmList[gv.LEFT_WRIST][2])
            right_hand = (self.lmList[gv.RIGHT_WRIST][1], self.lmList[gv.RIGHT_WRIST][2])
            
            if self.side == "LEFT":
                self.arms_top = self.isInSquare(left_hand, gv.TOP_AREA[0]) and self.isInSquare(right_hand, gv.TOP_AREA[0])
                self.arms_bottom = self.isInSquare(left_hand, gv.BOTTOM_AREA[0]) and self.isInSquare(right_hand, gv.BOTTOM_AREA[0])
                self.arms_side = self.isInSquare(left_hand, gv.LEFT_AREA[0]) and self.isInSquare(right_hand, gv.RIGHT_AREA[0])
                self.arms_left = self.isInSquare(left_hand, gv.LEFT_AREA[0]) and self.isInSquare(right_hand, gv.LEFT_AREA[0])
                self.arms_right = self.isInSquare(left_hand, gv.RIGHT_AREA[0]) and self.isInSquare(right_hand, gv.RIGHT_AREA[0])
            if self.side == "RIGHT":
                self.arms_top = self.isInSquare(left_hand, gv.TOP_AREA[1]) and self.isInSquare(right_hand, gv.TOP_AREA[1])
                self.arms_bottom = self.isInSquare(left_hand, gv.BOTTOM_AREA[1]) and self.isInSquare(right_hand, gv.BOTTOM_AREA[1])
                self.arms_side = self.isInSquare(left_hand, gv.LEFT_AREA[1]) and self.isInSquare(right_hand, gv.RIGHT_AREA[1])
                self.arms_left = self.isInSquare(left_hand, gv.LEFT_AREA[1]) and self.isInSquare(right_hand, gv.LEFT_AREA[1])
                self.arms_right = self.isInSquare(left_hand, gv.RIGHT_AREA[1]) and self.isInSquare(right_hand, gv.RIGHT_AREA[1])
        except Exception as e:
            if self.print_active:
                print(e)
            