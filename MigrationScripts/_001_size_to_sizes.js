db.Product.find().forEach(i => {
  db.Product.update({}, { $set: { 'shippingDetail.sizes': [i.shippingDetail.size] } });
});
db.Product.find().forEach(i => {
  db.Product.update({}, { $unset: { 'shippingDetail.size': '' } });
});
