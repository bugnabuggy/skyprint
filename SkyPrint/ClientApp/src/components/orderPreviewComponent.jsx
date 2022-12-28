import React from 'react';

import { PdfView } from './pdfView';
import { ImageView } from './imageView';

export class OrderPreview extends React.Component {

  render() {
    const pdfPreview = <PdfView
      order={this.props.order}
      className='margin-box'
    />;

    const imagePreview = <ImageView
      order={this.props.order}
      className='margin-box'
    />;

    return this.props.order.fileType === 'pdf'
      ? pdfPreview
      : imagePreview
  }


}