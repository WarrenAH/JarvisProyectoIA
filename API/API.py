import jsonpickle
import numpy as np
import pandas as pd
import statsmodels.api as sm
import pickle
from sklearn.preprocessing import StandardScaler
from flask import Flask, request, jsonify

# Cargar el modelo de regresión lineal desde el archivo
with open('.\Modelo\Modelo1.pkl', 'rb') as file:
    modelo_1 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('.\Modelo\Modelo2.pkl', 'rb') as file:
    modelo_2 = pickle.load(file)

# Cargar el modelo de regresión lineal desde el archivo
with open('.\Modelo\Modelo3.pkl', 'rb') as file:
    modelo_3 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('.\Modelo\Modelo4.pkl', 'rb') as file:
    modelo_4 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('.\Modelo\Modelo5.pkl', 'rb') as file:
    modelo_5 = pickle.load(file)

# Cargar el modelo de regresión lineal desde el archivo
with open('.\Modelo\Modelo6.pkl', 'rb') as file:
    modelo_6 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('.\Modelo\Modelo7.pkl', 'rb') as file:
    modelo_7 = pickle.load(file)

# Cargar el modelo KNN desde el archivo
with open('.\Modelo\Modelo8.pkl', 'rb') as file:
    modelo_8 = pickle.load(file)

# Cargar el modelo de regresión lineal desde el archivo
with open('.\Modelo\Modelo9.pkl', 'rb') as file:
    modelo_9 = pickle.load(file)

# Cargar el modelo de regresión lineal desde el archivo
with open('.\Modelo\Modelo10.pkl', 'rb') as file:
    modelo_10 = pickle.load(file)

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

# Ejecutar la aplicación Flask
if __name__ == '__main__':
    app.run(debug=True, host='127.0.0.1', port=5000)
