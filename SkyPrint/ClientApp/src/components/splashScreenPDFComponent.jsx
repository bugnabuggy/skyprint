import React from 'react';
import { Document, Page } from 'react-pdf';

export class SplashScreenPDFComponent extends React.Component {
  onDocumentLoadSuccess = ({ numPages }) => {
    this.setState({ numPages });
  }
  constructor(props) {
    super(props);
    this.state = {
      numPages: null,
      pageNumber: 2,
    };
  }
  renderPages = () => {
    let page = [];
    if (this.state.numPages) {
      for (let i = 1; i <= this.state.numPages; i++) {
        page.push(
          <Page key={`page_${i}`} pageNumber={i} className="document-pdf-page" />
        );
      }
    }
    return page;
  }
  render() {
    const style = {
      height: document.documentElement.clientHeight - 10,
    };
    return (
      <div className="splash-screen-document-container" onClick={this.props.handleDocumentClick}>
        <div className="splash-screen-document" style={style}>
          <Document
            file={this.props.file}
            onLoadSuccess={this.onDocumentLoadSuccess}
          >
            {this.renderPages()}
          </Document>
        </div>
      </div>
    );
  }
}