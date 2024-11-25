import sys
import cv2
from ultralytics import YOLO

# Argumentos: ruta del video y marca de tiempo en segundos
video_path = sys.argv[1]
timestamp = float(sys.argv[2])

# Cargar el modelo YOLO
model = YOLO("./best.pt")

# Abrir el video
cap = cv2.VideoCapture(video_path)

# Calcular el frame correspondiente a la marca de tiempo
fps = cap.get(cv2.CAP_PROP_FPS)
frame_number = int(fps * timestamp)
cap.set(cv2.CAP_PROP_POS_FRAMES, frame_number)

# Leer el frame en la marca de tiempo
ret, frame = cap.read()
if not ret:
    print("Error: no se pudo leer el frame.")
    sys.exit(1)

# Procesar el frame con YOLO
results = model(frame)
counts = {"explosive": 0, "handgun": 0, "rifle": 0}  # Cambia por tus clases

for result in results:
    for box in result.boxes.data:
        cls = int(box[5])  # Índice de clase
        class_name = model.names[cls]
        if class_name in counts:
            counts[class_name] += 1

# Mostrar los resultados
for cls, count in counts.items():
    print(f"{cls}: {count}")
