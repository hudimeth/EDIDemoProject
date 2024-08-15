import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import {Container, Button } from 'react-bootstrap'
import axios from 'axios';

const View855 = () => {

    const { id } = useParams();
    console.log(id);

    const [ediText, setEdiText] = useState('');

    const view855 = async () => {
        const { data } = await axios.get(`/api/ediconverter/view855/${id}`);
        setEdiText(data.ediText)
    }

    useEffect(() => {
        view855();
    }, [])

    return (
        <Container>
            <pre>{ediText}</pre>
        </Container>
        )
    
}

export default View855;