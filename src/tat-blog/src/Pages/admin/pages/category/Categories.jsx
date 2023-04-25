import { faTrash } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { useSelector } from "react-redux";
import { Link, useParams } from "react-router-dom";
import Loading from "../../../../Components/Loading";
import {
  deletCategory,
  getFilterCategory,
} from "../../../../Services/CategoriesWidgets";
import CategoryFilterPane from "../../../../Components/admin/CategoryFilterPane";

const Categories = () => {
  const [categoryList, setCategoryList] = useState([]);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true),
    categoryFilter = useSelector((state) => state.categoryFilter);

  let { id } = useParams,
    p = 1,
    ps = 10;

  useEffect(() => {
    document.title = "Quản lý chủ đế";

    getFilterCategory(categoryFilter, ps, p).then((data) => {
      if (data) {
        console.log("data category: ", data);
        setCategoryList(data.items);
      } else {
        setCategoryList([]);
      }
      setIsVisibleLoading(false);
    });
  }, [categoryFilter, p, ps]);

  // delete category api
  const handleDeleteCategory = (e, id) => {
    e.preventDefault();
    RemoveCategory(id);
    async function RemoveCategory(id) {
      if (window.confirm("Bạn có muốn xoá danh mục này")) {
        const response = await deletCategory(id);
        if (response) {
          alert("Đã xoá danh mục");
        } else alert("Đã xảy ra lỗi xoá danh mục này");
      }
    }
  };

  return (
    <>
      <h1>Đây là khu vực quản lý chủ đề {id}</h1>
      <CategoryFilterPane/>
      {isVisibleLoading ? (
        <Loading />
      ) : (
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th>Tên category</th>
              <th>Số bài viết</th>
              <th>Xoá</th>
            </tr>
          </thead>
          <tbody>
            {categoryList.length > 0 ? (
              categoryList.map((item, index) => (
                <tr key={index}>
                  <td>
                      <Link to={`/admin/categories/edit/${item.id}`}>
                        {item.name}
                      </Link>
                    <p className="text-muted">{item.description}</p>
                  </td>
                  <td>{item.postCount}</td>
                  <td>
                    <div
                      className="text-center text-danger"
                      onClick={(e) => handleDeleteCategory(e, item.id)}
                    >
                      <FontAwesomeIcon icon={faTrash} />
                    </div>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={3}>
                  <h4 className="text-danger text-center">
                    Không tìm thấy category nào
                  </h4>
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      )}
    </>
  );
};

export default Categories;
