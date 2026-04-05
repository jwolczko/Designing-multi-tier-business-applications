import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import type { AuthState, LoginResponse } from '../types/auth.types';

const initialState: AuthState = {
  token: null,
  userName: null,
  customerId: null,
  isAuthenticated: false,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setCredentials: (state, action: PayloadAction<LoginResponse>) => {
      state.token = action.payload.token;
      state.userName = action.payload.userName;
      state.customerId = action.payload.customerId;
      state.isAuthenticated = true;
    },
    clearCredentials: (state) => {
      state.token = null;
      state.userName = null;
      state.customerId = null;
      state.isAuthenticated = false;
    },
  },
});

export const { setCredentials, clearCredentials } = authSlice.actions;
export default authSlice.reducer;