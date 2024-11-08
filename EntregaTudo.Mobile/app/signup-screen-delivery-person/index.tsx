import React, { useState } from 'react';
import { StyleSheet, View, Text, TextInput, TouchableOpacity, ImageBackground, ScrollView, Alert, Modal, KeyboardAvoidingView, Platform } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { TextInputMask } from 'react-native-masked-text';
import RNPickerSelect from 'react-native-picker-select';
import logo from '../../assets/images/logo.jpg';
import { BASE_URL } from '../../config';

export default function SignupScreen() {
  const navigation = useNavigation();

  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [documentNumber, setDocumentNumber] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [vehicleBrand, setVehicleBrand] = useState('');
  const [vehicleLicensePlate, setVehicleLicensePlate] = useState('');
  const [vehicleLoadCapacity, setVehicleLoadCapacity] = useState('');
  const [vehicleManufactureYear, setVehicleManufactureYear] = useState('');
  const [vehicleModel, setVehicleModel] = useState('');
  const [vehicleType, setVehicleType] = useState('');
  const [signupSuccess, setSignupSuccess] = useState(false);
  const [userData, setUserData] = useState(null);
  const [showValidationError, setShowValidationError] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');

  const vehicleTypeOptions = [
    { label: 'Bicicleta', value: 0 },
    { label: 'Carro', value: 1 },
    { label: 'Motocicleta', value: 2 },
  ];

  const handleSignup = async () => {
    if (!firstName || !lastName || !documentNumber || !email || !phoneNumber || !username || !password || !vehicleBrand || !vehicleLicensePlate || !vehicleLoadCapacity || !vehicleManufactureYear || !vehicleModel || !vehicleType) {
      setErrorMessage('Por favor, preencha todos os campos.');
      setShowValidationError(true);
      return;
    }

    try {
      const payload = {
        FirstName: firstName,
        LastName: lastName,
        DocumentNumber: documentNumber,
        Email: email,
        PhoneNumber: phoneNumber,
        Username: username,
        Password: password,
        Vehicle: {
          Brand: vehicleBrand,
          LicensePlate: vehicleLicensePlate,
          LoadCapacity: vehicleLoadCapacity,
          ManufactureYear: vehicleManufactureYear,
          Model: vehicleModel,
          VehicleType: vehicleType,
        },
      };

      const response = await fetch(`${BASE_URL}/deliveryperson/create`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      });

      const data = await response.json();
      if (!response.ok) {
        const errorMessage = data.message || JSON.stringify(data);
        throw new Error(`Erro ao cadastrar usuário: ${errorMessage}`);
      }

      setUserData(data);
      setSignupSuccess(true);
    } catch (error) {
      Alert.alert('Erro', error.message);
    }
  };

  const renderSuccessMessage = () => (
    <View style={styles.successContainer}>
      <Text style={styles.successText}>Cadastro realizado com sucesso!</Text>
      <TouchableOpacity style={styles.successButton} onPress={() => navigation.navigate('login/index')}>
        <Text style={styles.successButtonText}>Ir para Login</Text>
      </TouchableOpacity>
    </View>
  );
  

  return (
    <KeyboardAvoidingView style={styles.flex} behavior={Platform.OS === 'ios' ? 'padding' : 'height'}>
      <ImageBackground source={logo} style={styles.background}>
        <View style={styles.overlay}>
          <ScrollView contentContainerStyle={styles.scrollContainer} keyboardShouldPersistTaps="handled">
            <Text style={styles.title}>Cadastro de Entregador</Text>
            {!signupSuccess ? (
              <>
                <View style={styles.section}>
                  <TextInput style={styles.input} placeholder="Nome" placeholderTextColor="#888" value={firstName} onChangeText={setFirstName} />
                  <TextInput style={styles.input} placeholder="Sobrenome" placeholderTextColor="#888" value={lastName} onChangeText={setLastName} />
                  <TextInputMask type={'cpf'} options={{ mask: '999.999.999-99' }} value={documentNumber} onChangeText={setDocumentNumber} style={styles.input} placeholder="Número do Documento (CPF/CNPJ)" placeholderTextColor="#888" keyboardType="numeric" />
                  <TextInput style={styles.input} placeholder="Email" placeholderTextColor="#888" value={email} onChangeText={setEmail} keyboardType="email-address" />
                  <TextInputMask type={'cel-phone'} options={{ maskType: 'BRL', withDDD: true, dddMask: '(99) ' }} value={phoneNumber} onChangeText={setPhoneNumber} style={styles.input} placeholder="Telefone" placeholderTextColor="#888" keyboardType="phone-pad" />
                  <TextInput style={styles.input} placeholder="Nome de Usuário" placeholderTextColor="#888" value={username} onChangeText={setUsername} />
                  <TextInput style={styles.input} placeholder="Senha" placeholderTextColor="#888" value={password} onChangeText={setPassword} secureTextEntry />
                </View>
                <View style={styles.section}>
                  <Text style={styles.sectionTitle}>Dados do Veículo</Text>
                  <TextInput style={styles.input} placeholder="Marca do Veículo" placeholderTextColor="#888" value={vehicleBrand} onChangeText={setVehicleBrand} />
                  <TextInput style={styles.input} placeholder="Placa do Veículo" placeholderTextColor="#888" value={vehicleLicensePlate} onChangeText={setVehicleLicensePlate} />
                  <TextInput style={styles.input} placeholder="Capacidade de Carga" placeholderTextColor="#888" value={vehicleLoadCapacity} onChangeText={setVehicleLoadCapacity} keyboardType="numeric" />
                  <TextInput style={styles.input} placeholder="Ano de Fabricação" placeholderTextColor="#888" value={vehicleManufactureYear} onChangeText={setVehicleManufactureYear} keyboardType="numeric" />
                  <TextInput style={styles.input} placeholder="Modelo do Veículo" placeholderTextColor="#888" value={vehicleModel} onChangeText={setVehicleModel} />
                  <RNPickerSelect
                    placeholder={{ label: "Selecione o Tipo de Veículo", value: null }}
                    onValueChange={(value) => setVehicleType(value)}
                    items={vehicleTypeOptions}
                    style={pickerSelectStyles}
                    value={vehicleType}
                  />
                </View>
                <TouchableOpacity style={styles.button} onPress={handleSignup}>
                  <Text style={styles.buttonText}>Cadastrar</Text>
                </TouchableOpacity>
              </>
            ) : renderSuccessMessage()}
          </ScrollView>

          <Modal visible={showValidationError} transparent={true} animationType="slide" onRequestClose={() => setShowValidationError(false)}>
            <View style={styles.modalContainer}>
              <View style={styles.modalContent}>
                <Text style={styles.modalText}>Erro</Text>
                <Text style={styles.modalMessage}>{errorMessage}</Text>
                <TouchableOpacity style={styles.closeButton} onPress={() => setShowValidationError(false)}>
                  <Text style={styles.closeButtonText}>Fechar</Text>
                </TouchableOpacity>
              </View>
            </View>
          </Modal>
        </View>
      </ImageBackground>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  flex: {
    flex: 1,
  },
  background: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  overlay: {
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    flex: 1,
    width: '100%',
  },
  scrollContainer: {
    padding: 20,
    alignItems: 'center',
    width: '100%', // Mantém a largura total
  },
  title: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 16,
  },
  section: {
    width: '100%',
    marginBottom: 20,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 10,
  },
  input: {
    width: '100%',
    backgroundColor: '#fff',
    padding: 10,
    marginBottom: 16,
    borderRadius: 5,
    color: '#000',
  },
  button: {
    backgroundColor: '#2196F3',
    padding: 15,
    borderRadius: 5,
    alignItems: 'center',
    marginTop: 20,
    width: '100%',
  },
  buttonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: 'bold',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
  },
  modalContent: {
    backgroundColor: '#fff',
    padding: 20,
    borderRadius: 8,
    alignItems: 'center',
    width: '80%',
  },
  modalText: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#ff4444',
    marginBottom: 10,
  },
  modalMessage: {
    fontSize: 16,
    color: '#333',
    textAlign: 'center',
    marginBottom: 20,
  },
  closeButton: {
    backgroundColor: '#2196F3',
    padding: 10,
    borderRadius: 5,
  },
  closeButtonText: {
    color: '#fff',
    fontSize: 16,
  },
  successContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
    backgroundColor: 'rgba(0, 0, 0, 0.7)',
    borderRadius: 10,
  },
  successText: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#4CAF50', // Verde para indicar sucesso
    textAlign: 'center',
    marginBottom: 20,
  },
  successButton: {
    backgroundColor: '#2196F3',
    paddingVertical: 12,
    paddingHorizontal: 30,
    borderRadius: 5,
    marginTop: 20,
  },
  successButtonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: 'bold',
    textAlign: 'center',
  },
});

const pickerSelectStyles = StyleSheet.create({
  inputIOS: {
    fontSize: 16,
    paddingVertical: 12,
    paddingHorizontal: 10,
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 5,
    color: '#000',
    backgroundColor: '#fff',
    paddingRight: 30,
    marginBottom: 16,
  },
  inputAndroid: {
    fontSize: 16,
    paddingHorizontal: 10,
    paddingVertical: 8,
    borderWidth: 0.5,
    borderColor: '#ccc',
    borderRadius: 5,
    color: '#000',
    backgroundColor: '#fff',
    paddingRight: 30,
    marginBottom: 16,
  },
});
