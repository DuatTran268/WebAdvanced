import { faTrash } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { useSelector } from "react-redux";
import { Link, useParams } from "react-router-dom";
import AuthorFilterPane from "../../../../Components/admin/AuthorFilterPane";
import Loading from "../../../../Components/Loading";
import {
  deleteAuthor,
  getAuthorById,
  getAuthorFilter,
} from "../../../../Services/TopAuthorWidget";

const Author = () => {
  const [authorList, setAuthorList] = useState([]);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true),
    authorFilter = useSelector((state) => state.authorFilter);

  let { id } = useParams,
    p = 1,
    ps = 10;

  useEffect(() => {
    document.title = "Quản lý tác giả";

    getAuthorFilter(authorFilter, ps, p).then((data) => {
      if (data) {
        setAuthorList(data.items);
      } else {
        setAuthorList([]);
      }
      setIsVisibleLoading(false);
    });
  }, [authorFilter, p, ps]);

  // delete
  const handleDeleteAuthor = (e, id) => {
    e.preventDefault();
    RemoveAuthor();

    async function RemoveAuthor(id) {
      if (window.confirm("Bạn có muốn xoá tác giả này")) {
        const res = await deleteAuthor(id);
        if (res) {
          alert("Đã xoá tác giả này");
        } else {
          alert("Đã xảy ra lỗi khi xoá tác giả này");
        }
      }
    }
  };

  return (
    <>
      <h1>Khu vực quản lý tác giả</h1>

      <AuthorFilterPane />
      {isVisibleLoading ? (
        <Loading />
      ) : (
        <Table striped responsive bordered>
          <thead>
            <tr>
              {/* <th>Avatar tác giả</th> */}
              <th>Tên tác giả</th>
              <th>Ngày tham gia</th>
              <th>Email</th>
              <th>Số bài viết</th>
              <th>Xoá</th>
            </tr>
          </thead>
          <tbody>
            {authorList.length > 0 ? (
              authorList.map((item, index) => (
                <tr key={index}>
                  {/* <td>
                        Hình
                      </td> */}
                  <td>
                    <Link to={`/admin/authors/edit/${item.id}`}>
                      {item.fullName}
                    </Link>
                  </td>
                  <td>{item.joinedDate}</td>
                  <td>{item.email}</td>
                  <td>{item.postCount}</td>
                  <td>
                    <div
                      className="text-center text-danger"
                      onClick={(e) => handleDeleteAuthor(e, item.id)}
                    >
                      <FontAwesomeIcon icon={faTrash} />
                    </div>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={6}>
                  <h4 className="text-danger text-center">
                    không tìm thấy tác giả nào
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

export default Author;
