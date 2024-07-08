import { StackNavigationProp } from '@react-navigation/stack';

export type RootStackParamList = {
    Home: undefined;
    SignupScreenCustomer: undefined;
    SignupScreenDeliveryPerson: undefined;
    Login: undefined;
    Profile: undefined; 
  };
  
  export type LoginScreenNavigationProp = StackNavigationProp<
  RootStackParamList,
  'Login'
>;

export type ProfileScreenNavigationProp = StackNavigationProp<
  RootStackParamList,
  'Profile'
>;

export type SignupScreenCustomerNavigationProp = StackNavigationProp<
  RootStackParamList,
  'SignupScreenCustomer'
>;

export type SignupScreenDeliveryPersonNavigationProp = StackNavigationProp<
  RootStackParamList,
  'SignupScreenDeliveryPerson'
>;