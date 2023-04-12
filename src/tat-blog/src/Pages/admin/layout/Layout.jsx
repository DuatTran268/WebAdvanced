import { Outlet } from "react-router-dom";
import Navbar from "../../../Components/admin/Navbar";

const AdminLayout = () => {
  return (
    <>
      <Navbar/>
      <div className="container-fluid py-3">
        <Outlet/>
      </div>
    </>
  );
}

export default AdminLayout;