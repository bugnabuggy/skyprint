import React from 'react';

export class ArrowComponent extends React.Component {
  render() {
    return(
      <div className="arrow-container">
        <img src="/Arrow.png" alt=""/>
        Нажмите на макет, чтобы увеличить
      </div>
    );
  }
}