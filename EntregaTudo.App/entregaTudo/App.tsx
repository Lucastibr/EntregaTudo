import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import HomeScreen from './HomeScreen';
import SignupScreenCustomer from './SignupScreenCustomer';
import SignupScreenDeliveryPerson from './SignupScreenDeliveryPerson';
import Login from './Login';
import Profile from './ProfileScreen';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator initialRouteName="Home">
        <Stack.Screen name="Home" component={HomeScreen} />
        <Stack.Screen name="Login" component={Login} />
        <Stack.Screen name="Profile" component={Profile} />
        <Stack.Screen name="SignupScreenCustomer" component={SignupScreenCustomer} />
        <Stack.Screen name="SignupScreenDeliveryPerson" component={SignupScreenDeliveryPerson} />
      </Stack.Navigator>
    </NavigationContainer>
  );
}
