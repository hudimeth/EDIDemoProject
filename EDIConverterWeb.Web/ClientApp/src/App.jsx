import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Route, Routes } from 'react-router';
import Create855 from './Pages/Create855';
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
                    <Route exact path='/create855' element={
                        <PrivateRoute>
                            <Create855 />
                        </PrivateRoute>
                    } />
                    <Route exact path='/view855/:id' element={
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