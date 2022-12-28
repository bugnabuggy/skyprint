import React, { useCallback, useState } from "react";
import { Document, Page } from 'react-pdf';

export function PdfView(props) {
    const {
        order,
        className,
        onClick
    } = props;

    const [state, setState] = useState({
        numPages: null,
        pageNumber: 0,
    });

    const documentLoaded = useCallback(({ numPages }) => {
        setState({ numPages });
    }, [setState])

    const renderPages = () => {
        if (state.numPages) {
            let page = [];
            for (let i = 1; i <= state.numPages; i++) {
                page.push(
                    <Page key={`page_${i}`} pageNumber={i} width={430} className="document-pdf-page" />
                );
            }
            return page;
        }
    }

    return (<div className={`document-pdf ${className || ''}`} onClick={onClick} >
        <Document
            file={order.picture}
            onLoadSuccess={documentLoaded}
        >
            {renderPages()}
        </Document>
    </div>)
}