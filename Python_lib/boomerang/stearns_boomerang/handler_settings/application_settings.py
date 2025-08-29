"""
.. currentmodule:: application_settings
.. moduleauthor:: Stearns Financial Center <SoftwareDevelopment@stearnsbank.com>

"""
import json
from typing import Any, TypeVar, Type, cast, Optional
from dataclasses import dataclass

T = TypeVar("T")

protocols = ["http", "https"]


def from_str(x: Any) -> str:
    """
    Validate if value is a string
    :param x:
    :return:
    """
    assert isinstance(x, str)
    return x


def from_none(x: Any) -> Any:
    """
    Make value nullable
    :param x:
    :return:
    """
    assert x is None
    return x


def from_union(fs, x):
    """

    :param fs:
    :param x:
    :return:
    """
    for f in fs:
        try:
            return f(x)
        except ValueError:
            pass
    assert False


def to_class(c: Type[T], x: Any) -> dict:
    """
    Convert to class
    :param c:
    :param x:
    :return:
    """
    assert isinstance(x, c)
    return cast(Any, x).to_dict()


def _convert(lst):
    """

    :param lst:
    :return:
    """
    dct = {}
    for i, v in enumerate(lst):
        if isinstance(v, tuple):
            for x in v:
                dct[x.lower()] = i
        else:
            dct[v.lower()] = i
    return dct


@dataclass
class ElasticSearch:
    """
    ElasticSearch DataModel
    """
    protocol: str
    host: str
    port: int

    @staticmethod
    def from_dict(obj: Any) -> 'ElasticSearch':
        """
        Method to convert to the DataModel
        :param obj:
        :return:
        """
        assert isinstance(obj, dict)
        protocol = from_str(obj.get("Protocol"))
        host = from_str(obj.get("Host"))
        port = int(from_str(obj.get("Port")))
        return ElasticSearch(protocol, host, port)

    def to_dict(self) -> dict:
        """
        Method to convert from the DataModel
        :return:
        """
        result: dict = {"Protocol": from_str(self.protocol), "Host": from_str(self.host),
                        "Port": from_str(str(self.port))}
        return result


@dataclass
class ReportingService:
    """
    ElasticSearch
    """
    elastic_search: ElasticSearch
    document_type: Optional[str]
    source: Optional[str]
    host_url: str

    @staticmethod
    def from_dict(obj: Any) -> 'ReportingService':
        """
        Method to convert to the DataModel
        :param obj:
        :return:
        """
        assert isinstance(obj, dict)
        elastic_search = ElasticSearch.from_dict(obj.get("ElasticSearch"))
        document_type = from_union([from_str, from_none], obj.get("DocumentType"))
        source = from_union([from_str, from_none], obj.get("Source"))
        host_url = elastic_search.protocol + "://" + elastic_search.host + ":" + str(elastic_search.port)
        return ReportingService(elastic_search, document_type, source, host_url)

    def to_dict(self) -> dict:
        """
        Method to convert from the DataModel
        :return:
        """
        result: dict = {"ElasticSearch": to_class(ElasticSearch, self.elastic_search),
                        "DocumentType": from_union([from_str, from_none], self.document_type),
                        "Source": from_union([from_str, from_none], self.source)
                        }
        return result


@dataclass
class SettingsValidation:
    """
    SettingsValidation DataModel
    """
    reporting_service: ReportingService

    @staticmethod
    def from_dict(obj: Any) -> 'SettingsValidation':
        """
        Method to convert to the DataModel
        :param obj:
        :return:
        """
        assert isinstance(obj, dict)
        reporting_service = ReportingService.from_dict(obj.get("ReportingService"))
        return SettingsValidation(reporting_service)

    def to_dict(self) -> dict:
        """
        Method to convert from the DataModel
        :return:
        """
        result: dict = {"ReportingService": to_class(ReportingService, self.reporting_service)}
        return result


def settings_validation_from_dict(s: Any) -> SettingsValidation:
    """
    Parse json to DataModel
    :param s:
    :return:
    """
    return SettingsValidation.from_dict(s)


def settings_validation_to_dict(x: SettingsValidation) -> Any:
    """
    Convert DataModel to Dict
    :param x:
    :return:
    """
    return to_class(SettingsValidation, x)


def settings_validation_from_json(j: json) -> SettingsValidation:
    """
    Parse json object to DataModel
    :param j:
    :return:
    """
    return settings_validation_from_dict(json.load(j))


def settings_validation_from_json_config(c: Any) -> SettingsValidation:
    """
    Parse json file to DataModel
    :param c:
    :return:
    """
    with open(c, 'r') as reader:
        o = settings_validation_from_json(reader)
    return o
