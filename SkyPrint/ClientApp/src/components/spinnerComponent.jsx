import React from 'react';
import MDSpinner from "react-md-spinner";

export function Spinner(props) {
  return(
    <div className="load-spinner">
      <h1 className="number_order">Загрузка данных</h1>
      <MDSpinner 
        size={50}
      />
    </div>
  );
}