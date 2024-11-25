import jsonpickle
import numpy as np
import pandas as pd
import statsmodels.api as sm
import pickle
import base64
import io
import typing
from sklearn.preprocessing import StandardScaler
from flask import Flask, request, jsonify
from keras.models import load_model
from tensorflow.keras.utils import load_img, img_to_array
from PIL import Image
from mltu.inferenceModel import OnnxInferenceModel
from mltu.preprocessors import WavReader
from mltu.utils.text_utils import ctc_decoder
from mltu.configs import BaseModelConfigs

# Cargar el modelo de regresión lineal desde el archivo
with open('./Modelo/Modelo1.pkl', 'rb') as file:
    modelo_1 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('./Modelo/Modelo2.pkl', 'rb') as file:
    modelo_2 = pickle.load(file)

# Cargar el modelo de regresión lineal desde el archivo
with open('./Modelo/Modelo3.pkl', 'rb') as file:
    modelo_3 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('./Modelo/Modelo4.pkl', 'rb') as file:
    modelo_4 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('./Modelo/Modelo5.pkl', 'rb') as file:
    modelo_5 = pickle.load(file)

# Cargar el modelo de regresión lineal desde el archivo
with open('./Modelo/Modelo6.pkl', 'rb') as file:
    modelo_6 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('./Modelo/Modelo7.pkl', 'rb') as file:
    modelo_7 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('./Modelo/Modelo8.pkl', 'rb') as file:
    modelo_8 = pickle.load(file)

# Cargar el modelo de regresión lineal desde el archivo
with open('./Modelo/Modelo9.pkl', 'rb') as file:
    modelo_9 = pickle.load(file)

# Cargar el modelo de regresión lineal desde el archivo
with open('./Modelo/Modelo10.pkl', 'rb') as file:
    modelo_10 = pickle.load(file)

#######################################################

# Cargar el ModeloCara y el mapa de resultados
Modelo_cara = load_model('./ModeloCara/model.h5')
with open('./ModeloCara/ResultMap.pkl', 'rb') as file:
    result_map_modelo_cara = pickle.load(file)

class WavToTextModel(OnnxInferenceModel):
    def __init__(self, char_list: typing.Union[str, list], *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.char_list = char_list

    def predict(self, data: np.ndarray):
        data_pred = np.expand_dims(data, axis=0)
        preds = self.model.run(None, {self.input_name: data_pred})[0]
        text = ctc_decoder(preds, self.char_list)[0]
        return text

# Cargar el ModeloVoz
configs = BaseModelConfigs.load("./ModeloVoz/202411051328/configs.yaml")
model = WavToTextModel(model_path=configs.model_path, char_list=configs.vocab, force_cpu=False)

# Crear una función para predecir el precio del automóvil
def predecir_automovil(data):
    # Convertir los datos a un array de NumPy y asegurar que tenga la forma correcta
    data_array = np.array([data['Selling_Price'], data['Kms_Driven']])

    # Asegurarse de que el array sea 2D para añadir la constante correctamente
    data_array = data_array.reshape(1, -1)

    # Añadir la constante (intercepto)
    data_array_const = sm.add_constant(data_array, has_constant='add')

    # Hacer la predicción
    prediction = modelo_1.predict(data_array_const)

    # Retornar la predicción
    return prediction[0]

# Función para clasificar la calidad del vino
def clasificar_calidad_vino(data):
    scaler = StandardScaler()
    # Convertir los datos a un array de NumPy
    data_array = np.array([[
        data['type'],
        data['fixed acidity'],
        data['volatile acidity'],
        data['citric acid'],
        data['residual sugar'],
        data['chlorides'],
        data['free sulfur dioxide'],
        data['total sulfur dioxide'],
        data['density'],
        data['pH'],
        data['sulphates'],
        data['alcohol']
    ]])

    # Convertir el data_array en un DataFrame para que sea compatible con el modelo
    data_array_df = pd.DataFrame(data_array)

    # Escalar los datos (asume que ya has entrenado el escalador)
    data_array_scaled = scaler.fit_transform(data_array_df)

    # Hacer la predicción
    prediccion = modelo_2.predict(data_array_scaled)

    # Retornar la predicción
    return int(prediccion[0])

# Crear una función para predecir el precio del aguacate
def predecir_aguacate(data):
    # Convertir los datos a un array de NumPy y asegurar que tenga la forma correcta
    data_array = np.array([data['Type'], data['Month'], data['Day'], data['Year']])

    # Asegurarse de que el array sea 2D para añadir la constante correctamente
    data_array = data_array.reshape(1, -1)

    # Añadir la constante (intercepto)
    data_array_const = sm.add_constant(data_array, has_constant='add')

    # Hacer la predicción
    prediction = modelo_3.predict(data_array_const)

    # Retornar la predicción
    return prediction[0]

# Función para clasificar el tipo de cirrosis
def clasificar_tipo_cirrosis(data):
    scaler = StandardScaler()
    # Convertir los datos a un array de NumPy
    data_array = np.array([[
        data['N_Days'],
        data['Status'],
        data['Drug'],
        data['Age'],
        data['Sex'],
        data['Ascites'],
        data['Hepatomegaly'],
        data['Spiders'],
        data['Edema'],
        data['Bilirubin'],
        data['Cholesterol'],
        data['Albumin'],
        data['Copper'],
        data['Alk_Phos'],
        data['SGOT'],
        data['Tryglicerides'],
        data['Platelets'],
        data['Prothrombin']
    ]])

    # Convertir el data_array en un DataFrame para que sea compatible con el modelo
    data_array_df = pd.DataFrame(data_array)

    # Escalar los datos (asume que ya has entrenado el escalador)
    data_array_scaled = scaler.fit_transform(data_array_df)

    # Hacer la predicción
    prediccion = modelo_4.predict(data_array_scaled)

    # Retornar la predicción
    return int(prediccion[0])

# Función para clasificar el tipo de hepatitis
def clasificar_tipo_hepatitis(data):
    scaler = StandardScaler()
    # Convertir los datos a un array de NumPy
    data_array = np.array([[
        data['Age'],
        data['Sex'],
        data['ALB'],
        data['ALP'],
        data['ALT'],
        data['AST'],
        data['BIL'],
        data['CHE'],
        data['CHOL'],
        data['CREA'],
        data['GGT'],
        data['PROT']
    ]])

    # Convertir el data_array en un DataFrame para que sea compatible con el modelo
    data_array_df = pd.DataFrame(data_array)

    # Escalar los datos (asume que ya has entrenado el escalador)
    data_array_scaled = scaler.fit_transform(data_array_df)

    # Hacer la predicción
    prediccion = modelo_5.predict(data_array_scaled)

    # Retornar la predicción
    return int(prediccion[0])

# Crear una función para predecir la masa corporal
def predecir_masa_corporal(data):
    # Convertir los datos a un array de NumPy y asegurar que tenga la forma correcta
    data_array = np.array([data['Thigh'], data['Hip'], data['Abdomen'], data['Weight']])

    # Asegurarse de que el array sea 2D para añadir la constante correctamente
    data_array = data_array.reshape(1, -1)

    # Añadir la constante (intercepto)
    data_array_const = sm.add_constant(data_array, has_constant='add')

    # Hacer la predicción
    prediction = modelo_6.predict(data_array_const)

    # Retornar la predicción
    return prediction[0]

# Función para clasificar si un paciente tendra un accidente cerebro-vascular
def clasificar_accidente_cerebro_vascular(data):
    scaler = StandardScaler()
    # Convertir los datos a un array de NumPy
    data_array = np.array([[
        data['gender'],
        data['age'],
        data['hypertension'],
        data['heart_disease'],
        data['ever_married'],
        data['work_type'],
        data['Residence_type'],
        data['avg_glucose_level'],
        data['bmi'],
        data['smoking_status']
    ]])


    # Convertir el data_array en un DataFrame para que sea compatible con el modelo
    data_array_df = pd.DataFrame(data_array)

    # Escalar los datos (asume que ya has entrenado el escalador)
    data_array_scaled = scaler.fit_transform(data_array_df)

    # Hacer la predicción
    prediccion = modelo_7.predict(data_array_scaled)

    # Retornar la predicción
    return int(prediccion[0])

# Función para clasificar si un cliente se pasara de compañia telefonica
def clasificar_cliente_telefonica(data):
    scaler = StandardScaler()
    # Convertir los datos a un array de NumPy
    data_array = np.array([[
        data['gender'],
        data['SeniorCitizen'],
        data['Partner'],
        data['Dependents'],
        data['tenure'],
        data['PhoneService'],
        data['MultipleLines'],
        data['InternetService'],
        data['OnlineSecurity'],
        data['OnlineBackup'],
        data['DeviceProtection'],
        data['TechSupport'],
        data['StreamingTV'],
        data['StreamingMovies'],
        data['Contract'],
        data['PaperlessBilling'],
        data['PaymentMethod'],
        data['MonthlyCharges'],
        data['TotalCharges']
    ]])

    # Convertir el data_array en un DataFrame para que sea compatible con el modelo
    data_array_df = pd.DataFrame(data_array)

    # Escalar los datos (asume que ya has entrenado el escalador)
    data_array_scaled = scaler.fit_transform(data_array_df)

    # Hacer la predicción
    prediccion = modelo_8.predict(data_array_scaled)

    # Retornar la predicción
    return int(prediccion[0])

# Crear una función para predecir el precio del bitcoin
def predecir_bitcoin(data):
    # Convertir los datos a un array de NumPy y asegurar que tenga la forma correcta
    data_array = np.array([data['Open'], data['High'], data['Low']])

    # Asegurarse de que el array sea 2D para añadir la constante correctamente
    data_array = data_array.reshape(1, -1)

    # Añadir la constante (intercepto)
    data_array_const = sm.add_constant(data_array, has_constant='add')

    # Hacer la predicción
    prediction = modelo_9.predict(data_array_const)

    # Retornar la predicción
    return prediction[0]

# Crear una función para predecir el indice de precio de los clientes de Walmart
def predecir_indice_precio_walmart(data):
    # Convertir los datos a un array de NumPy y asegurar que tenga la forma correcta
    data_array = np.array([data['Dept'], data['Month']])

    # Asegurarse de que el array sea 2D para añadir la constante correctamente
    data_array = data_array.reshape(1, -1)

    # Añadir la constante (intercepto)
    data_array_const = sm.add_constant(data_array, has_constant='add')

    # Hacer la predicción
    prediction = modelo_10.predict(data_array_const)

    # Retornar la predicción
    return prediction[0]

#######################################################

# Función que procesa la imagen de la cara y hace la clasificacion
def clasificar_modelo_cara(image_base64):
    # Decodifica la imagen de base64
    image_data = base64.b64decode(image_base64)
    image = Image.open(io.BytesIO(image_data)).resize((256, 256))
    image_array = img_to_array(image)
    image_array = np.expand_dims(image_array, axis=0)

    # Realiza la clasificacion con el modelo
    result = Modelo_cara.predict(image_array, verbose=0)
    predicted_class_index = np.argmax(result)
    predicted_class_name = result_map_modelo_cara[predicted_class_index]

    # Devuelve el nombre de la clase y las probabilidades
    return predicted_class_name, result.tolist()

def reconocimiento_modelo_voz(audio_base64):
    # Decodificar la cadena base64
    audio_data = base64.b64decode(audio_base64)

    # Convertir el audio en un objeto de archivo en memoria
    audio_file = io.BytesIO(audio_data)

    # Leer el espectrograma del archivo de audio
    spectrogram = WavReader.get_spectrogram(audio_file, frame_length=configs.frame_length, frame_step=configs.frame_step, fft_length=configs.fft_length)

    # Asegurarse de que el espectrograma tenga la longitud adecuada
    padded_spectrogram = np.pad(spectrogram, ((configs.max_spectrogram_length - spectrogram.shape[0], 0), (0, 0)), mode='constant', constant_values=0)

    # Realizar la predicción
    predicted_text = model.predict(padded_spectrogram)

    return predicted_text

# Flask App
app = Flask(__name__)

# Endpoint para predecir el precio de un automovil
@app.route('/api/predecir_automovil', methods=['POST'])
def automovil():
    # Obtener los datos de la solicitud JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función predecir_automovil para hacer la predicción
    prediction = predecir_automovil(data)

    # Retornar la predicción en formato JSON
    return jsonify({'prediccion_automovil': prediction})

# Endpoint para clasificar la calidad del vino
@app.route('/api/clasificar_calidad_vino', methods=['POST'])
def vino():
    # Recibir los datos en formato JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función de predicción
    prediccion = clasificar_calidad_vino(data)

    # Retornar la predicción en formato JSON
    return jsonify({'clasificacion_calidad_vino': prediccion})

# Endpoint para predecir el precio de un automovil
@app.route('/api/predecir_aguacate', methods=['POST'])
def aguacate():
    # Obtener los datos de la solicitud JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función predecir_aguacate para hacer la predicción
    prediction = predecir_aguacate(data)

    # Retornar la predicción en formato JSON
    return jsonify({'prediccion_aguacate': prediction})

# Endpoint para clasificar el tipo de cirrosis
@app.route('/api/clasificar_tipo_cirrosis', methods=['POST'])
def cirrosis():
    # Recibir los datos en formato JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función de predicción
    prediccion = clasificar_tipo_cirrosis(data)

    # Retornar la predicción en formato JSON
    return jsonify({'clasificacion_tipo_cirrosis': prediccion})

# Endpoint para clasificar el tipo de hepatitis
@app.route('/api/clasificar_tipo_hepatitis', methods=['POST'])
def hepatitis():
    # Recibir los datos en formato JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función de predicción
    prediccion = clasificar_tipo_hepatitis(data)

    # Retornar la predicción en formato JSON
    return jsonify({'clasificacion_tipo_hepatitis': prediccion})

# Endpoint para predecir la masa corporal
@app.route('/api/predecir_masa_corporal', methods=['POST'])
def masa_corporal():
    # Obtener los datos de la solicitud JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función predecir_masa_corporal para hacer la predicción
    prediction = predecir_masa_corporal(data)

    # Retornar la predicción en formato JSON
    return jsonify({'prediccion_masa_corporal': prediction})

# Endpoint para clasificar si un paciente tendra un accidente cerebro-vascular
@app.route('/api/clasificar_accidente_cerebro_vascular', methods=['POST'])
def cerebro_vascular():
    # Recibir los datos en formato JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función de predicción
    prediccion = clasificar_accidente_cerebro_vascular(data)

    # Retornar la predicción en formato JSON
    return jsonify({'clasificacion_accidente_cerebro_vascular': prediccion})

# Endpoint para clasificar si un cliente se pasara de compañia telefonica
@app.route('/api/clasificar_cliente_telefonica', methods=['POST'])
def cliente_telefonica():
    # Recibir los datos en formato JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función de predicción
    prediccion = clasificar_cliente_telefonica(data)

    # Retornar la predicción en formato JSON
    return jsonify({'clasificacion_cliente_telefonica': prediccion})

# Endpoint para predecir el precio del bitcoin
@app.route('/api/predecir_bitcoin', methods=['POST'])
def bitcoin():
    # Obtener los datos de la solicitud JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función predecir_bitcoin para hacer la predicción
    prediction = predecir_bitcoin(data)

    # Retornar la predicción en formato JSON
    return jsonify({'prediccion_bitcoin': prediction})

# Endpoint para predecir el indice de precio de los clientes de Walmart
@app.route('/api/predecir_indice_precio_walmart', methods=['POST'])
def indice_precio_walmart():
    # Obtener los datos de la solicitud JSON
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    # Llamar a la función predecir_indice_precio_walmart para hacer la predicción
    prediction = predecir_indice_precio_walmart(data)

    # Retornar la predicción en formato JSON
    return jsonify({'prediccion_indice_precio_walmart': prediction})

#######################################################

@app.route('/api/clasificar_modelo_cara', methods=['POST'])
def modelo_cara():
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    if 'image' not in data:
        return jsonify({'error': 'No image provided'}), 400
    
    image_base64 = data['image']
    
    # Llamamos a la función que procesa la imagen y hace la clasificacion
    predicted_class_name, class_probabilities = clasificar_modelo_cara(image_base64)

    # Devuelve la respuesta en formato JSON
    return jsonify({
        'clasificar_modelo_cara': predicted_class_name})

@app.route('/api/reconocimiento_modelo_voz', methods=['POST'])
def modelo_voz():
    data = request.get_json()
    print(f"Datos recibidos: {data}")

    if 'audio' not in data:
        return jsonify({'error': 'No audio provided'}), 400

    audio_base64 = data['audio']

    predicted_text = reconocimiento_modelo_voz(audio_base64)

    return jsonify({'reconocimiento_modelo_voz': predicted_text})

# Ejecutar la aplicación Flask
if __name__ == '__main__':
    app.run(debug=True, host='192.168.0.18', port=5000)
