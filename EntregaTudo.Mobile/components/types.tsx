import { StackNavigationProp } from '@react-navigation/stack';

export type RootStackParamList = {
  Home: undefined;
  SignupScreenCustomer: undefined;
  SignupScreenDeliveryPerson: undefined;
  Login: undefined;
  Profile: undefined; 
  'select-item/index': undefined;
  EnterAddress: { item: string };
  DeliveryDetails: { item: string; address: string };
};

export type HomeScreenNavigationProp = StackNavigationProp<
  RootStackParamList,
  'Home'
>;

export type SignupScreenCustomerNavigationProp = StackNavigationProp<
  RootStackParamList,
  'SignupScreenCustomer'
>;

export type SignupScreenDeliveryPersonNavigationProp = StackNavigationProp<
  RootStackParamList,
  'SignupScreenDeliveryPerson'
>;

export type LoginScreenNavigationProp = StackNavigationProp<
  RootStackParamList,
  'Login'
>;

export type ProfileScreenNavigationProp = StackNavigationProp<
  RootStackParamList,
  'Profile'
>;

export type SelectItemScreenNavigationProp = StackNavigationProp<
  RootStackParamList,
  'select-item/index'
>;

export type EnterAddressScreenNavigationProp = StackNavigationProp<
  RootStackParamList,
  'EnterAddress'
>;

export type DeliveryDetailsScreenNavigationProp = StackNavigationProp<
  RootStackParamList,
  'DeliveryDetails'
>;
