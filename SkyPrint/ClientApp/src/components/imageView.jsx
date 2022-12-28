import React from 'react';

export function ImageView(props) {
    const {
        order,
        className,
        onClick
    } = props;

    return <div className={`maket-image-container ${className}`}>
        <img
            className="maket-image cursor-normal"
            src={order.picture}
            alt=""
            onClick={onClick}
        />
    </div>
}