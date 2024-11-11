import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Route, Routes } from 'react-router';
import AddPurchaseOrder from './Pages/AddPurchaseOrder';
import Home from './Pages/Home';
//import AddUser from './Pages/Account/AddUser';
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
                    {/*<Route exact path='/adduser' element={<AddUser />} />*/}
                    <Route exact path='/login' element={<Login />} />
                    <Route exact path='/addpurchaseorder' element={
                        <PrivateRoute>
                            <AddPurchaseOrder />
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