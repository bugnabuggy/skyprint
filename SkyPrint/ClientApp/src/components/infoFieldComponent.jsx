import React from 'react';
import { SplashScreen } from './splashScreenComponent';
import { InfoTable } from './infoTable';
import { ButtonsComponent } from './buttonsComponent';

export class InfoField extends React.Component {
  handleImageClick = () => {
    this.setState({ showPicture: !this.state.showPicture });
  };
  
  constructor(props) {
    super(props);
    this.state = {
      showPicture: false,
    };
    this.handleImageClick = this.handleImageClick.bind(this);
  }
  render() {
    return (
      <div className="info_field">
        <div>
          <p className="number_order">№ ЗАКАЗА: {this.props.order.name}</p>
          {/* {this.props.order.hasClientAnswer && 
          this.props.order.status &&
          <Alert bsStyle="success">{this.props.order.status}</Alert>} */}
          <InfoTable
            order={this.props.order}
          />
        </div>
        <div>
          <img
            className="maket-image"
            src={this.props.order.picture}
            alt=""
            onClick={this.handleImageClick}
          />
          {this.state.showPicture &&
            <SplashScreen
              image={this.props.order.picture}
              imageClick={this.handleImageClick}
              showPicture={this.state.showPicture}
            />
          }
        </div>
        <div>
          <ButtonsComponent
            order={this.props.order}
            showAmendmentsModalAction={this.props.showAmendmentsModalAction}
            showApprovedModalAction={this.props.showApprovedModalAction}
          />
        </div>
      </div>
    );
  }
}

