import React from 'react';
import { SplashScreen } from './splashScreenComponent';
import { InfoTable } from './infoTable';
import { ButtonsComponent } from './buttonsComponent';
import { SplashScreenPDFComponent } from './splashScreenPDFComponent';
import { Document, Page } from 'react-pdf';

export class InfoField extends React.Component {
  handleImageClick = () => {
    this.setState({ showPicture: !this.state.showPicture });
  };
  handleDocumentClick = () => {
    this.setState({ showDocument: !this.state.showDocument });
  };
  onDocumentLoadSuccess = ({ numPages }) => {
    this.setState({ numPages });
  };
  handleSplashScreenBanner = () => {
    this.props.showSplashScreenBannerAction(false);
  };

  constructor(props) {
    super(props);
    this.state = {
      showPicture: false,
      showDocument: false,
      numPages: null,
      pageNumber: 2,
    };
    this.handleImageClick = this.handleImageClick.bind(this);
  }
  renderPages = () => {
    if (this.state.numPages) {
      let page = [];
      for (let i = 1; i <= this.state.numPages; i++) {
        page.push(
          <Page key={`page_${i}`} pageNumber={i} width={430} className="document-pdf-page" />
        );
      }
      return page;
    }
  }
  render() {
    return (
      <div className="info_field">
        <div>
          <strong><p className="number_order">№ ЗАКАЗА: {this.props.order.name}</p></strong>
          {/* {this.props.order.hasClientAnswer && 
          this.props.order.status &&
          <Alert bsStyle="success">{this.props.order.status}</Alert>} */}
          <InfoTable
            order={this.props.order}
          />
        </div>
        <div>
          {this.props.order.fileType === 'pdf' ?
            (<React.Fragment>
              <div className="document-pdf" onClick={this.handleDocumentClick}>
                <Document
                  file={this.props.order.picture}
                  onLoadSuccess={this.onDocumentLoadSuccess}
                >
                  {this.renderPages()}
                </Document>
              </div>
              {this.state.showDocument &&
                <SplashScreenPDFComponent
                  file={this.props.order.picture}
                  handleDocumentClick={this.handleDocumentClick}
                />
              }
            </React.Fragment>)
            :
            (
              <React.Fragment>
                <div className="maket-image-container">
                  <img
                    className="maket-image"
                    src={this.props.order.picture}
                    alt=""
                    onClick={this.handleImageClick}
                  />
                </div>
                {this.state.showPicture &&
                  <SplashScreen
                    image={this.props.order.picture}
                    imageClick={this.handleImageClick}
                    showPicture={this.state.showPicture}
                  />
                }
              </React.Fragment>)
          }
          {this.props.order.isShowBanner &&
            (
              <SplashScreen
                image="/api/banner/center"
                imageClick={this.handleSplashScreenBanner}
                showPicture={this.props.order.isShowBanner}
              />
            )}
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

