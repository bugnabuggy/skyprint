import React from 'react';

export function OrderNotFound(props) {
  return (
    <div className="not_found_page">
      <h1 className="number_order">{props.name}</h1>
    </div>
  )
}
