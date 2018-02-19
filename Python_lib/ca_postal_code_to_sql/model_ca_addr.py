
class CAPostalCode:

    __slots__ = ['postal_code', 'city', 'place_names', 'province']

    def __init__(

            self,
            postal_code,
            city,
            place_names,
            province
    ):
        self.postal_code = postal_code
        self.city = city
        self.place_names = place_names
        self.province = province
