import React from 'react';

export class SplashScreen extends React.Component {

  componentDidMount() {
    window.onkeyup = (event) => {
      if (this.props.showPicture && event.keyCode === 27) {
        this.props.imageClick();
      }
    }
  }
  componentWillUnmount() {
    window.onkeyup = null;
  }
  render() {
    return (
      <div
        className="splash-screen"
        onClick={this.props.imageClick}
      >
        <img
          src={this.props.image}
          className="img-thumbnail splash-screen-image"
        />
      </div>
    );
  }
}