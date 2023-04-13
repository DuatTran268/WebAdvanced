import React, {useEffect} from "react";
import { Image } from "react-bootstrap";

const NotFound = () => {
  useEffect(() => {
    document.title = "404 Not Found";
  })

  return  (
    <div className="text-center">
    <Image src="https://cdn.tgdd.vn/hoi-dap/580732/loi-404-not-found-la-gi-9-cach-khac-phuc-loi-404-not-3-800x534.jpg"/>
    </div>
  );
}

export default NotFound;