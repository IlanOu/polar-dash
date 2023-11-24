import cv2
import os
import shutil
from datetime import datetime
import globals_vars as gv

def compress_and_save_image(image):
    # Compresser l'image en PNG
    success, encoded_image = cv2.imencode('.png', image)

    if success:
        # Récupérer le répertoire actuel du script
        script_directory = os.path.dirname(os.path.abspath(__file__))

        # Définir le chemin vers le dossier StreamingAssets du projet Unity
        unity_streaming_assets_path = os.path.join(script_directory, gv.STREAMING_ASSETS_PATH)

        # Vérifier si le dossier StreamingAssets existe, sinon le créer
        if not os.path.exists(unity_streaming_assets_path):
            os.makedirs(unity_streaming_assets_path)

        # Obtenir la date et l'heure actuelles
        current_datetime = datetime.now()

        # Formatage de la date et l'heure dans le format souhaité pour le nom de fichier
        formatted_datetime = current_datetime.strftime("%Y%m%d_%H%M%S")

        # Définir le nom du fichier dans le dossier StreamingAssets avec la date et l'heure
        destination_filename = f"image_{formatted_datetime}.png"
        
        # Définir le chemin complet de destination dans le dossier StreamingAssets
        destination_path = os.path.join(unity_streaming_assets_path, destination_filename)

        try:
            # Écrire l'image compressée dans le fichier de destination
            with open(destination_path, 'wb') as file:
                encoded_image.tofile(file)
            print(f"L'image a été compressée et sauvegardée avec succès dans {destination_path}")
            check_and_delete_old_images(unity_streaming_assets_path)
        except Exception as e:
            print(f"Erreur lors de la sauvegarde de l'image compressée : {e}")
    else:
        print("Erreur lors de la compression de l'image")

def check_and_delete_old_images(folder_path):
    # Récupérer la liste des fichiers dans le dossier
    files = os.listdir(folder_path)

    # Trier les fichiers par date de modification (les plus anciens en premier)
    files.sort(key=lambda x: os.path.getmtime(os.path.join(folder_path, x)))

    # Vérifier le nombre d'images
    num_images = len(files)

    # Supprimer la première image si le nombre dépasse la limite
    if num_images > gv.MAX_IMAGE:
        file_to_delete = os.path.join(folder_path, files[0])
        os.remove(file_to_delete)
        print(f"Suppression de l'image la plus ancienne : {file_to_delete}")