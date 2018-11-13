import React from 'react';
import MDSpinner from "react-md-spinner";

export function Spinner(props) {
  return(
    <div>
      Загрузка данных 
      <MDSpinner 
        size={50}
      />
    </div>
  );
}