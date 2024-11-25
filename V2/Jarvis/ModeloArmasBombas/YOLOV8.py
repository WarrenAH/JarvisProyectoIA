import sys
import cv2
import os
from ultralytics import YOLO

# Argumentos: ruta del video
video_path = sys.argv[1]

# Cargar el modelo YOLO
model = YOLO("../../ModeloArmasBombas/best.pt")

# Crear la carpeta "resultados"
output_folder = "../../ModeloArmasBombas/resultados"
if os.path.exists(output_folder):
    # Eliminar la carpeta y su contenido
    for root, dirs, files in os.walk(output_folder, topdown=False):
        for file in files:
            os.remove(os.path.join(root, file))
        for dir in dirs:
            os.rmdir(os.path.join(root, dir))
    os.rmdir(output_folder)
# Crear la carpeta nuevamente
os.makedirs(output_folder, exist_ok=True)

# Definir colores para cada clase
class_colors = {
    "explosive": (0, 0, 255),  # Rojo
    "handgun": (255, 0, 0),    # Azul
    "rifle": (0, 128, 0)       # Verde
}

# Abrir el video
cap = cv2.VideoCapture(video_path)

# Obtener la cantidad de frames por segundo
fps = cap.get(cv2.CAP_PROP_FPS)

# Definir el contador global para cada clase
total_counts = {"explosive": 0, "handgun": 0, "rifle": 0}

# Variables para el seguimiento
frame_count = 0
second_interval = 5  # Intervalo de 5 segundos

# Definir el archivo de resultados
result_file_path = "../../ModeloArmasBombas/resultados/resultados.txt"

# Eliminar el archivo de resultados si existe y crear uno nuevo
if os.path.exists(result_file_path):
    os.remove(result_file_path)

# Abrir el archivo en modo de escritura
with open(result_file_path, "w") as result_file:

    while True:
        ret, frame = cap.read()
        if not ret:
            break

        # Cada vez que llegamos al segundo especificado (5, 10, 15, etc.), procesamos la detección
        if frame_count % int(fps * second_interval) == 0:
            # Calcular el tiempo en minutos y segundos
            time_in_seconds = frame_count / fps
            minutes = int(time_in_seconds // 60)
            seconds = int(time_in_seconds % 60)

            # Procesar el frame con YOLO
            results = model(frame)
            counts = {"explosive": 0, "handgun": 0, "rifle": 0}  # Contadores locales para esta iteración

            # Procesar los resultados de la detección
            for result in results:
                for box in result.boxes.data:
                    x1, y1, x2, y2, conf, cls = box[:6]  # Coordenadas y clase
                    x1, y1, x2, y2 = map(int, (x1, y1, x2, y2))
                    class_name = model.names[int(cls)]
                    if class_name in counts:
                        counts[class_name] += 1
                        total_counts[class_name] += 1  # Sumar al total global

                    # Determinar el color para esta clase
                    color = class_colors.get(class_name, (255, 255, 255))  # Blanco por defecto

                    # Dibujar el cuadro alrededor del objeto detectado
                    cv2.rectangle(frame, (x1, y1), (x2, y2), color, 2)

                    # Dibujar el fondo del texto
                    text = f"{class_name} ({conf:.2f})"
                    (font_scale, thickness) = (1.0, 2)  # Escala y grosor del texto
                    (text_width, text_height), baseline = cv2.getTextSize(text, cv2.FONT_HERSHEY_SIMPLEX, font_scale, thickness)

                    # Ampliar el fondo alrededor del texto con un margen adicional
                    margin = 10  # Margen adicional para el fondo
                    text_bg_width = text_width + margin * 2
                    text_bg_height = text_height + margin

                    # Evitar que el fondo se dibuje fuera de la imagen
                    text_bg_x1 = max(x1, 0)
                    text_bg_y1 = max(y1 - text_bg_height - 4, 0)
                    text_bg_x2 = min(x1 + text_bg_width, frame.shape[1])
                    text_bg_y2 = min(y1, frame.shape[0])

                    # Dibujar el fondo del texto
                    cv2.rectangle(frame, 
                                (text_bg_x1, text_bg_y1), 
                                (text_bg_x2, text_bg_y2), 
                                color, 
                                cv2.FILLED)

                    # Dibujar la etiqueta del objeto
                    text_x = text_bg_x1 + margin
                    text_y = text_bg_y2 - margin
                    cv2.putText(frame, text, (text_x, text_y), cv2.FONT_HERSHEY_SIMPLEX, font_scale, (255, 255, 255), thickness)

            # Mostrar los resultados de la detección en el tiempo calculado
            result_file.write(f"En el minuto {minutes}:{seconds:02d} se encontraron los siguientes objetos:\n")
            for cls, count in counts.items():
                result_file.write(f"{count} {cls}(s) en este segundo\n")

            # Mostrar el total acumulado hasta el momento
            result_file.write("\nTotales acumulados hasta el momento:\n")
            for cls, total in total_counts.items():
                result_file.write(f"{total} {cls}(s) encontrados en total\n")
            result_file.write("-" * 50 + "\n")

            # Guardar el frame procesado
            output_path = os.path.join(output_folder, f"{minutes:02d}_{seconds:02d}.jpg")
            cv2.imwrite(output_path, frame)

        frame_count += 1

# Liberar el video
cap.release()
