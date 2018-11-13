import React from 'react';
import Header from './components/headerComponent';
import Footer from './components/footerComponent';
import Main from './components/mainComponent';

export default ( props ) => (
    <React.Fragment>
        <Header />
        <Main />
        <Footer />
    </React.Fragment>
);
