import React, { useState, useEffect, createContext, useContext } from 'react';
import axios from 'axios';

const AuthContext = createContext();

const AuthContextComponent = ({ children }) => {
    const [user, setUser] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    const loadUser = async () => {
        const { data } = await axios.get('/api/account/getcurrentuser');
        setUser(data);
        setIsLoading(false);
    }

    useEffect(() => {
        loadUser();
    }, []);

    if (isLoading) {
        return (
            <div className='container pt-5'>
                <div className="text-center">
                    <div className="spinner-border" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                    <h4>Loading content...</h4>
                </div>
            </div>
        )
    }

    return (
        <AuthContext.Provider value={{ user, setUser }}>
            {children}
        </AuthContext.Provider>
    )
}
const useAuth = () => useContext(AuthContext);

export { AuthContextComponent, useAuth };