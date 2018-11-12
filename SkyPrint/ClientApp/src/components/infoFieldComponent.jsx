import React from 'react';
import { Alert } from 'react-bootstrap';
import { SplashScreen } from './splashScreenComponent';
import { MakeEdits } from './modalMakeEditsComponent';

export class InfoField extends React.Component {
  handleImageClick = () => {
    this.setState({ showPicture: !this.state.showPicture });
  };
  handleApprove = (event) => {
    console.log('handleApprove');
  };
  handleDownload = (event) => {
    console.log('handleDownload');
  };
  openAmendments = () => {
    this.props.showAmendmentsModalAction(true);
  };
  closeAmendments = () => {
    this.props.showAmendmentsModalAction(false);
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
        <h1 className="number_order">№ ЗАКАЗА: {this.props.order.name}</h1>
        {this.props.order.hasClientAnswer && <Alert bsStyle="success">{this.props.order.status}</Alert>}
        <p className="notification">Утверждая макет в пучать. Заказчик подтверждает, что вся информация на макете верна, размеры соответствуют требованию заказчика, пожелания к дизайну учтены.</p>
        <p className="notification">После утверждения макета все возможные претензии по поводу содержаний, дизайна, размеров макета от заказчика не принимаются.</p>
        <div>
          <img
            className="maket-image"
            src={this.props.order.picture}
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
        <div className="o-btn-container">
          {!this.props.order.hasClientAnswer &&
            <div
              name="approve"
              onClick={this.handleApprove}
            >
              <span className="o-btn-success o-btn-info-field">
                Утвердить в печать
            </span>
            </div>}
          <div
            name="download"
            className="download-file"
            onClick={this.handleDownload}
          >
            <a
              href={this.props.order.picture}
              className="o-btn-primary o-btn-info-field"
              target="_blank"
            >
              Скачать файл
            </a>
          </div>
          <div
            name="amendments"
            onClick={this.openAmendments}
          >
            <span className="o-btn-primary o-btn-info-field">
              Внести правки >
            </span>
          </div>
        </div>
        <MakeEdits
          showAmendments={this.props.order.isShowAmendmentsModal}
          orderId={this.props.orderId}
          closeModal={this.closeAmendments}
          sendAmendments={this.props.sendAmendments}
        />
      </div>
    );
  }
}

