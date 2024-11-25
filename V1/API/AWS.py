import os
import boto3

def entrenamiento():
    s3 = boto3.resource('s3')

    # carpeta de entrenamiento
    carpeta_entrenamiento = './Entrenamiento/'

    # lista para almacenar las imágenes y sus nombres
    imagenes = []

    # iterar sobre los archivos en la carpeta de entrenamiento
    for filename in os.listdir(carpeta_entrenamiento):
        # combinar la ruta de la carpeta de entrenamiento con el nombre del archivo
        filepath = os.path.join(carpeta_entrenamiento, filename)
        # obtener el nombre del archivo sin la extensión
        nombre = os.path.splitext(filename)[0]
        # reemplazar los guiones bajos (_) con espacios en el nombre del archivo
        nombre = nombre.replace('_', ' ')
        # añadir la tupla (ruta del archivo, nombre) a la lista de imagenes
        imagenes.append((filepath, nombre))

    # iterar a traves de la lista para cargar objetos en S3
    for imagen in imagenes:
        archivo = open(imagen[0],'rb')
        objecto = s3.Object('actores','index/'+ imagen[0])
        ret = objecto.put(Body=archivo,
                        Metadata={'FullName':imagen[1]})

entrenamiento()
