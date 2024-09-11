import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Route, Routes } from 'react-router';
import Add850 from './Pages/Add850';
import Home from './Pages/Home';
import View855 from './Pages/View855';
import AddUser from './Pages/Account/AddUser';
import Login from './Pages/Account/Login';
import { AuthContextComponent } from './Components/Authentication/AuthContextComponent';
import PrivateRoute from './Components/Authentication/PrivateRoute';
import Logout from './Components/Authentication/Logout';
import Layout from './Components/Layout';

const App = () => {
    return (
        <AuthContextComponent>
            <Layout>
                <Routes>
                    <Route exact path='/' element={
                        <PrivateRoute>
                            <Home />
                        </PrivateRoute>
                    } />
                    <Route exact path='/adduser' element={<AddUser />} />
                    <Route exact path='/login' element={<Login />} />
                    <Route exact path='/add850' element={
                        <PrivateRoute>
                            <Add850 />
                        </PrivateRoute>
                    } />
                    <Route exact path='/view855/:referenceNumber' element={
                        <PrivateRoute>
                            <View855 />
                        </PrivateRoute>
                    } />
                    <Route exact path='/logout' element={
                        <PrivateRoute>
                            <Logout />
                        </PrivateRoute>
                    } />
                </Routes>
            </Layout>
        </AuthContextComponent>
    )
}

export default App;