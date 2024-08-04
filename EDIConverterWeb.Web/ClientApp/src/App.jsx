import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Route, Routes } from 'react-router';
import Create855 from './Pages/Create855';
import Home from './Pages/Home';
import View855 from './Pages/View855';

const App = () => {
    return (
            <Routes>
                <Route exact path='/' element={<Home />} />
            <Route exact path='/create855' element={<Create855 />} />
            <Route exact path='/view855/:id' element={<View855 />} />
            </Routes>
    )
}

export default App;