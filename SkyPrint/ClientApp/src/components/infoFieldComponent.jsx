import React from 'react';

import { SplashScreen } from './splashScreenComponent';
import { InfoTable } from './infoTable';
import { ButtonsComponent } from './buttonsComponent';
import { SplashScreenPDFComponent } from './splashScreenPDFComponent';

import { PdfView } from './pdfView';
import { ImageView } from './imageView';

export class InfoField extends React.Component {
  handleImageClick = () => {
    this.setState({ showPicture: !this.state.showPicture });
  };
  handleDocumentClick = () => {
    this.setState({ showDocument: !this.state.showDocument });
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

  render() {
    const pdfView = <React.Fragment>
      <PdfView
        order={this.props.order}
        onClick={this.handleDocumentClick} />
      {this.state.showDocument &&
        <SplashScreenPDFComponent
          file={this.props.order.picture}
          handleDocumentClick={this.handleDocumentClick}
        />
      }
    </React.Fragment>;

    const imgView = <React.Fragment>
      <ImageView
        order={this.props.order}
        onClick={this.handleImageClick}
      />
      {this.state.showPicture &&
        <SplashScreen
          image={this.props.order.picture}
          imageClick={this.handleImageClick}
          showPicture={this.state.showPicture}
        />
      }
    </React.Fragment>;


    return (
      <div className="info_field">
        <div>
          <strong><p className="number_order">№ ЗАКАЗА: {this.props.order.name}</p></strong>
          <InfoTable
            order={this.props.order}
          />
        </div>
        <div>
          {this.props.order.fileType === 'pdf'
            ? pdfView
            : imgView
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

