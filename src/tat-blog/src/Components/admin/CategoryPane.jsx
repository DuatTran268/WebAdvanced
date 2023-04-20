import { useEffect, useState } from "react"
import { Button } from "react-bootstrap";
import { useDispatch, useSelector } from "react-redux"
import { Form, Link } from "react-router-dom";
import { reset, updateKeyword } from "../../Redux/Reducer";
import { getFilterCategory } from "../../Services/CategoriesWidgets";


const CategoryFilterPane = () => {
  const categoryFilter = useSelector(state => state.categoryFilter), dispatch = useDispatch(),
  [filter, setFilter] = useState();

  const handleReset = (e) => {
    dispatch(reset());
  }

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  useEffect(() => {
    getFilterCategory().then((data) => {
      if (data){
        setFilter(data);
      }
      else{
        setFilter([]);
      }
    })
  }, [])

  return(
    <Form method="get"
    onReset={handleReset}
    onSubmit={handleSubmit}
    className="row gy-2 gx-3 align-items-center p-2"
    >
    <Form.Group className="col-auto">
      <Form.Label className="visually-hidden">
        Keyword
      </Form.Label>
      <Form.Control 
      type="text"
      placeholder="Nhập vào từ khoá ..."
      name="name"
      value={categoryFilter}
      onChange = {(e) => dispatch(updateKeyword(e.target.value))}
      />
    </Form.Group>

    <Form.Group className="col-auto">
        <Button variant="danger" type="submit">
          Xoá Lọc
        </Button>
        <Button  type="submit" className="btn btn-primary ms-2">
          Bỏ lọc
        </Button>
        <Link to='/admin/categories/edit' className="btn btn-success ms-2">Thêm mới</Link>
      </Form.Group>
    </Form>
  );

}

export default CategoryFilterPane;