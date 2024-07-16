import React, { useState } from 'react';
import { StyleSheet, View, Text, TextInput, Button, ImageBackground } from 'react-native';
import logo from './images/logo.jpg';

export default function SignupScreen() {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [documentNumber, setDocumentNumber] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [signupSuccess, setSignupSuccess] = useState(false);
  const [userData, setUserData] = useState(null);

  const handleSignup = () => {
    fetch('http://10.0.2.2:5240/customer/create', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        FirstName: firstName,
        LastName: lastName,
        DocumentNumber: documentNumber,
        Email: email,
        PhoneNumber: phoneNumber,
        username: username,
        password: password,
      }),
    })
    .then(response => {
      if (!response.ok) {
        throw new Error('Erro ao cadastrar usuário');
      }
      return response.json();
    })
    .then(data => {
      console.log('deu certo!');
      setUserData(data); 
      setSignupSuccess(true);
    })
    .catch((error) => {
      console.error('Erro:', error);
    });
  };

  const renderSuccessMessage = () => {
    return (
      <View style={styles.successMessage}>
        <Text style={styles.successText}>Cadastro realizado com sucesso!</Text>
        <Text>Nome: {userData.FirstName} {userData.LastName}</Text>
        <Text>Email: {userData.Email}</Text>
        <Text>Telefone: {userData.PhoneNumber}</Text>
      </View>
    );
  };

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.title}>Cadastro</Text>
        {!signupSuccess ? (
          <>
            <TextInput
              style={styles.input}
              placeholder="Nome"
              value={firstName}
              onChangeText={setFirstName}
            />
            <TextInput
              style={styles.input}
              placeholder="Sobrenome"
              value={lastName}
              onChangeText={setLastName}
            />
            <TextInput
              style={styles.input}
              placeholder="Número do Documento"
              value={documentNumber}
              onChangeText={setDocumentNumber}
            />
            <TextInput
              style={styles.input}
              placeholder="Email"
              value={email}
              onChangeText={setEmail}
              keyboardType="email-address"
            />
            <TextInput
              style={styles.input}
              placeholder="Telefone"
              value={phoneNumber}
              onChangeText={setPhoneNumber}
              keyboardType="phone-pad"
            />
            <TextInput
              style={styles.input}
              placeholder="Nome de Usuário"
              value={username}
              onChangeText={setUsername}
            />
            <TextInput
              style={styles.input}
              placeholder="Senha"
              value={password}
              onChangeText={setPassword}
              secureTextEntry
            />
            <Button title="Cadastrar" onPress={handleSignup} />
          </>
        ) : renderSuccessMessage()}
      </View>
    </ImageBackground>
  );
}

const styles = StyleSheet.create({
  background: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  overlay: {
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: '100%',
    padding: 20,
  },
  title: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 16,
  },
  input: {
    width: '100%',
    backgroundColor: '#fff',
    padding: 10,
    marginBottom: 16,
    borderRadius: 5,
  },
  successMessage: {
    alignItems: 'center',
  },
  successText: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 16,
  },
});